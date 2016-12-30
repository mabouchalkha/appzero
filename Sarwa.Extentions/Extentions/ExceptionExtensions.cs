using System.Text;

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
