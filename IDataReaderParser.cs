using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Tao.Repository
{
    public interface IDataReaderParser
    {
        List<T> Parse<T>(IDataReader reader) where T: new();

        Task<List<T>> ParseAsync<T>(DbDataReader reader, CancellationToken cancellationToken) where T : new();
    }
}
