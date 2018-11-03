private Task<List<T>> BuildResult<T>(Type objectType, DbDataReader reader, CancellationToken cancellationToken)
            where T: new()
        {
            List<T> result = new List<T>();
            var typeReflector = objectType.GetTypeReflector(TypeReflectorCreationStrategy.PREPARE_DATA_RECORD_CONSTRUCTOR);

            if (typeReflector.DataRecordConstructor == null)
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add((T)ReadNextObject(typeReflector, reader));
                }
            }
            else
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add((T)typeReflector.DataRecordConstructor(reader));
                }
            }

            return result;
        }

        private static object ReadNextObject(TypeReflector typeReflector, IDataRecord reader)
        {
            // Create a new Object
            object newObjectToReturn;

            if (reader.FieldCount != 0)
            {
                newObjectToReturn = typeReflector.NewInstance();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string propertyName = reader.GetName(i);
                    SingleMemberSetter prop;
                    if (!typeReflector.TryGetSetter(propertyName, out prop) || prop == null)
                    {
                        // The technique is not very reliable, since the same DTO can be used in multiple queries. The current implementation only records the missing properties for the first query.
                        // Should be good enough for debugging, though.
                        //todo:
                        //UpdateMissingProperties(newObjectToReturn.GetType(), propertyName);
                        continue;
                    }

                    object value = reader[propertyName];
                    //
                    // Special processing when we have boolean targets and string sources.
                    //
                    if (value is string)
                    {
                        switch (prop.TypeCode)
                        {
                            case TypeCode.Boolean:
                                value = ((string)value).ParseBoolean();
                                break;
                            case TypeCode.Char:
                                value = ((string)value).Length == 0 ? '\0' : ((string)value)[0];
                                break;
                            default:
                                if (prop.TypeCode != TypeCode.String && string.IsNullOrWhiteSpace((string)value))
                                {
                                    value = DBNull.Value;
                                }
                                break;
                        }

                        if (typeof(Version) == prop.PropertyType)
                        {
                            value = Version.Parse((string)value);
                        }
                    }
                    if (value != DBNull.Value)
                    {
                        // 
                        // Special processing in case there is a type mismatch on integral types.
                        // These can throw an OverflowException if you botch up the types, so it's 
                        // best to avoid allowing this method to downsize the type. Also, at this 
                        // point we know it's not null, so we don't have to deal with Nullable<>.
                        //
                        switch (prop.TypeCode)
                        {
                            case TypeCode.Int16:
                                if (!(value is short))
                                {
                                    value = Convert.ToInt16(value);
                                }
                                break;
                            case TypeCode.Int32:
                                if (!(value is int))
                                {
                                    value = Convert.ToInt32(value);
                                }
                                break;
                            case TypeCode.Int64:
                                if (!(value is long))
                                {
                                    value = Convert.ToInt64(value);
                                }
                                break;
                            case TypeCode.Boolean:
                                value = Convert.ToBoolean(value);
                                break;
                        }

                        prop.Setter(newObjectToReturn, value);
                    }
                }

                //todo:
                //DoneUpdatingMissingProperties(newObjectToReturn.GetType());
            }
            else
            {
                // There are no properties for this type. It's probably a scalar type, which we will be nice and support.
                // We assume column 0 has the value. These can throw if you really botch up the types in the query.
                newObjectToReturn = reader[0];
            }
            return newObjectToReturn;
        }