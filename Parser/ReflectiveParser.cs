using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public class ReflectiveParser: IDataReaderParser
    {
        public List<T> Parse<T>(IDataReader reader)
            where T : new()
        {
            List<T> result = new List<T>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    BuildResult<T>(reader, result);
                }
            }

            return result;
        }

        public async Task<List<T>> ParseAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
            where T : new()
        {
            List<T> result = new List<T>();

            if (reader != null)
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    BuildResult<T>(reader, result);
                }
            }

            return result;
        }

        private void BuildResult<T>(IDataReader reader, List<T> result)
            where T : new()
        {
            T t = new T();
            Hashtable cacheProperties = GetProperties<T>();
            for (int idx = 0; idx < reader.FieldCount; idx++)
            {
                PropertyInfo info = (PropertyInfo)cacheProperties[reader.GetName(idx).ToUpper()];
                if ((info != null) && info.CanWrite)
                {
                    info.SetValue(t, reader.GetValue(idx), null);
                }
            }
            result.Add(t);
        }

        private Hashtable GetProperties<T>()
        {
            Hashtable result = new Hashtable();

            Type entityType = typeof(T);
            PropertyInfo[] properties = entityType.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                result[info.Name.ToUpper()] = info;
            }

            return result;
        }
    }
}
