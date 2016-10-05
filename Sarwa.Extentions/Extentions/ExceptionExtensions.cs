using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ExceptionExtensions
    {
        public static string FullMessage(this Exception ex)
        {
            var builder = new StringBuilder();

            while (ex != null)
            {
                builder.AppendFormattedLine(ex.Message);
                ex = ex.InnerException;
            }

            return builder.ToString();
        }
    }
}
