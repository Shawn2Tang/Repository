using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository.Impl
{
    public partial class OracleRepository : AbstractRepository, IRepository
    {
        private ParserFactory parserFactory;

        public OracleRepository()
        {
            this.parserFactory = new ParserFactory();
        }

        public override List<T> Get<T>(
            string dbConnString,
            string sql,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (OracleConnection conn = new OracleConnection(dbConnString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
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
            using (OracleConnection conn = new OracleConnection(dbConnString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
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
            using (OracleConnection conn = new OracleConnection(dbConnString))
            {
                using (OracleCommand command = new OracleCommand(sql, conn))
                {
                    return await base.GetAsync<T>(conn, command, cancellationToken, dataParameters, commandBehavior);
                }
            }
        }
    }
}
