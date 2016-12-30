using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sarwa.Core.Data.EFCore.Helpers
{
    public static class DbSetExtensions
    {
        public static async Task<IReadOnlyList<T>> FromReaderAsync<T>(this DbSet<T> set, DbDataReader reader) where T : class
        {
            var r = new List<T>();

            var materializer = new DbDataReaderMaterializer<T>(set);
            while (await reader.ReadAsync())
                r.Add(materializer.Materialize(reader));

            return r.AsReadOnly();
        }

        public static IReadOnlyList<T> FromReader<T>(this DbSet<T> set, DbDataReader reader) where T : class
        {
            var r = new List<T>();

            var materializer = new DbDataReaderMaterializer<T>(set);
            while (reader.Read())
                r.Add(materializer.Materialize(reader));

            return r.AsReadOnly();
        }


        private struct DbDataReaderMaterializer<T>
        {
            private readonly Func<ValueBuffer, T> materializer;
            private readonly IRelationalValueBufferFactory valueBufferFactory;

            public DbDataReaderMaterializer(IInfrastructure<IServiceProvider> accessor)
            {
                var valueBufferParameter = Expression.Parameter(typeof(ValueBuffer));
                materializer = Expression.Lambda<Func<ValueBuffer, T>>(
                    accessor.GetService<IEntityMaterializerSource>().CreateMaterializeExpression(
                        accessor.GetService<IModel>().FindEntityType(typeof(T)),
                        valueBufferParameter),
                    valueBufferParameter).Compile();

                valueBufferFactory = accessor.GetService<IRelationalValueBufferFactoryFactory>().Create(new[] { typeof(T) }, null);
            }

            public T Materialize(DbDataReader currentRecord)
            {
                return materializer.Invoke(valueBufferFactory.Create(currentRecord));
            }
        }
    }
}
