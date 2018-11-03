using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public partial class SqlRepository: AbstractRepository, IRepository
    {

        public override List<T> Get<T>(
            string dbConnString,
            string sql,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (SqlConnection conn = new SqlConnection(dbConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return base.Get<T>(conn, cmd, dataParameters, commandTimeout, commandBehavior);
                }
            }
        }

        public override IEnumerable<T> Get<T>(
            string dbConnString,
            string sql,
            Func<IDataRecord, T> projector,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (SqlConnection conn = new SqlConnection(dbConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return base.Get<T>(conn, cmd, projector, dataParameters, commandTimeout, commandBehavior);
                }
            }
        }

        public override async Task<IEnumerable<T>> GetAsync<T>(
            string dbConnString,
            string sql,
            CancellationToken cancellationToken,
            IList<IDataParameter> dataParameters = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (SqlConnection conn = new SqlConnection(dbConnString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    return await base.GetAsync<T>(conn, command, cancellationToken, dataParameters, commandBehavior);
                }
            }
        }
    }
}
