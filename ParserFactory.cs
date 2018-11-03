using System;

namespace Tao.Repository
{
    public class ParserFactory
    {
        public IDataReaderParser GetParser<T>()
        {
            var objectType = typeof(T);
            if (objectType.IsPrimitive
                || objectType == typeof(string)
                || objectType == typeof(decimal)
                || objectType == typeof(DateTime)
                || objectType == typeof(Guid))
            {
                return new PrimitiveParser();
            }
            else
            {
                return new ReflectiveParser();
            }
        }
    }
}
