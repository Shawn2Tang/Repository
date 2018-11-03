using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public class PrimitiveParser: IDataReaderParser
    {
        public List<T> Parse<T>(IDataReader reader)
        {
            List<T> result = new List<T>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    result.Add((T)reader[0]);
                }
            }
            return result;
        }

        public async Task<List<T>> ParseAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
        {
            List<T> result = new List<T>();
            if (reader != null)
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add((T)reader[0]);
                }
            }

            return result;
        }
    }
}
