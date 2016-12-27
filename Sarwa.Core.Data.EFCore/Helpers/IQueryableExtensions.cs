using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Sarwa.Core.Data.EFCore.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sort == null)
            {
                return source;
            }

            // split the sort string
            var lstSort = sort.Split(',').Select(s => s.Trim()).ToArray();

            // run through the sorting options and create a sort expression string from them

            string completeSortExpression = "";
            foreach (var sortOption in lstSort)
            {
                // if the sort option starts with "-", we order
                // descending, otherwise ascending

                if (sortOption.StartsWith("-"))
                {
                    completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";
                }
                else
                {
                    completeSortExpression = completeSortExpression + sortOption + ",";
                }

            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));
            }

            return source;
        }
    }
}
