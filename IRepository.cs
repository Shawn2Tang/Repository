using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public interface IRepository
    {
        List<T> Get<T>(
            string dbConnString,
            string sql,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T : new();

        IEnumerable<IDataRecord> GetDatRecords(
            string dbConnString,
            string sql,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

        IEnumerable<T> Get<T>(
            string dbConnString,
            string sql,
            Func<IDataRecord, T> projector,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

        Task<IEnumerable<T>> GetAsync<T>(
            string dbConnString,
            string sql,
            CancellationToken cancellationToken,
            IList<IDataParameter> dataParameters = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T : new();
    }
}
