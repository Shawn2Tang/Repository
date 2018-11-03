using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    partial class SqlRepository
    {
        public async Task<IEnumerable<T>> GetAsync<T>(
            string dbConnString,
            string sql,
            CancellationToken cancellationToken,
            IList<IDataParameter> dataParameters = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T: new()
        {
            using (SqlConnection conn = new SqlConnection(dbConnString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    PrepareParameters(command, dataParameters);
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        var parser = parserFactory.GetParser<T>();
                        var result = await parser.ParseAsync<T>(reader, cancellationToken);
                        reader.Close();
                        return result;
                    }
                }
            }
        }
    }
}
