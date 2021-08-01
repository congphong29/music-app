using System.Collections.Generic;

namespace Source.Middleware
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public long Total { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}