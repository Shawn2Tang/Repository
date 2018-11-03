using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public class RepositoryBase
    {
        private ParserFactory parserFactory;

        public RepositoryBase()
        {
            this.parserFactory = new ParserFactory();
        }

        protected IEnumerable<T> Get<T>(
            IDbConnection connection,
            IDbCommand command,
            Func<IDataRecord, T> projector,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            return Get<T>(
                connection,
                command,
                (IDataRecord rdr) => { return projector(rdr); },
                dataParameters,
                commandTimeout,
                commandBehavior);
        }

        protected List<T> Get<T>(
            IDbConnection connection,
            IDbCommand command,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T: new()
        {
            List<T> result = new List<T>();

            connection.Open();
            command.CommandTimeout = commandTimeout;
            this.PrepareParameters(command, dataParameters);
            using (var reader = command.ExecuteReader(commandBehavior))
            {
                var parser = parserFactory.GetParser<T>();
                result = parser.Parse<T>(reader);

                reader.Close();
            }

            if (commandBehavior == CommandBehavior.CloseConnection)
            {
                connection.Close();
            }

            return result;
        }

        private IEnumerable<T> Get<T>(
            IDbConnection connection,
            IDbCommand command,
            Func<IDataReader, T> project,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            connection.Open();
            command.CommandTimeout = commandTimeout;
            PrepareParameters(command, dataParameters);
            using (var reader = command.ExecuteReader(commandBehavior))
            {
                while (reader.Read())
                {
                    yield return project(reader);
                }

                reader.Close();
            }

            if (commandBehavior == CommandBehavior.CloseConnection)
            {
                connection.Close();
            }
        }

        protected async Task<List<T>> GetAsync<T>(
            DbConnection connection,
            DbCommand command,
            CancellationToken cancellationToken,
            IList<IDataParameter> dataParameters = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T: new()
        {
            var result = new List<T>();
            await connection.OpenAsync();
            PrepareParameters(command, dataParameters);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                var parser = parserFactory.GetParser<T>();
                result = await parser.ParseAsync<T>(reader, cancellationToken);
                reader.Close();
            }

            if (commandBehavior == CommandBehavior.CloseConnection)
            {
                connection.Close();
            }

            return result;
        }

        protected void PrepareParameters(IDbCommand command, IList<IDataParameter> dataParameters)
        {
            if (dataParameters != null)
            {
                foreach (IDataParameter item in dataParameters)
                {
                    var param = command.CreateParameter();
                    param.DbType = item.DbType;
                    param.Direction = item.Direction;
                    param.ParameterName = item.ParameterName;
                    param.Value = item.Value;
                }
            }
        }        
    }
}
