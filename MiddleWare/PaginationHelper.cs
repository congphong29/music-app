using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Source.Middleware
{
    public static class DataPagerExtension
    {
        public static int GetStartRow(int page, int size)
        {
            return (page - 1) * size + 1;
        }

        public static int GetEndRow(int page, int size)
        {
            return (page - 1) * size + size;
        }

        public static int GetSkipRow(int page, int size)
        {
            return GetStartRow(page, size) - 1;
        }
    }
}