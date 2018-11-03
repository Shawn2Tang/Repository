using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Retrieve data synchronously.
        /// Automatically convert to correct type by C# reflective mechanism.
        /// </summary>
        /// <typeparam name="T">Generic type which the result list will contain.</typeparam>
        /// <param name="dbConnString">Database connection string</param>
        /// <param name="sql">Given sql to retrieve data from database</param>
        /// <param name="dataParameters">Given parameters which will be passed into sql.</param>
        /// <param name="commandTimeout">Maximum sql execution time.</param>
        /// <param name="commandBehavior">Default is closing connection.</param>
        /// <returns></returns>
        List<T> Get<T>(
            string dbConnString,
            string sql,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T : new();

        /// <summary>
        /// Retrieve data synchronously.
        /// Custom converter is used to convert to correct generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnString"></param>
        /// <param name="sql"></param>
        /// <param name="projector">Custom converter to transfer IDataRecord into correct type.</param>
        /// <param name="dataParameters"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandBehavior"></param>
        /// <returns></returns>
        IEnumerable<T> Get<T>(
            string dbConnString,
            string sql,
            Func<IDataRecord, T> projector,
            IList<IDataParameter> dataParameters = null,
            int commandTimeout = 10000,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

        /// <summary>
        /// Retrieve data asynchronously.
        /// Automatically convert to correct type by C# reflective mechanism.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnString"></param>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="dataParameters"></param>
        /// <param name="commandBehavior"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync<T>(
            string dbConnString,
            string sql,
            CancellationToken cancellationToken,
            IList<IDataParameter> dataParameters = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
            where T : new();
    }
}
