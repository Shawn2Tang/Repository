using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository.Impl
{
    public class MySqlRepository : AbstractRepository, IRepository
    {
        public override List<T> Get<T>(
            string dbConnString, 
            string sql, IList<IDataParameter> dataParameters = null, 
            int commandTimeout = 10000, 
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (var conn = new MySqlConnection(dbConnString))
            {
                using (var cmd = new MySqlCommand(sql, conn))
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
            using (var conn = new MySqlConnection(dbConnString))
            {
                using (var cmd = new MySqlCommand(sql, conn))
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
            using (var conn = new MySqlConnection(dbConnString))
            {
                using (var command = new MySqlCommand(sql, conn))
                {
                    return await base.GetAsync<T>(conn, command, cancellationToken, dataParameters, commandBehavior);
                }
            }
        }
    }
}
