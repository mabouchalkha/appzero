using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Sarwa.Core.Data.EFCore
{
    public interface IDataRepositoryBase
    {
    }

    public interface IDataRepositoryBase<TEntity, UContext, TKey> : IDataRepositoryBase
        where TEntity : class, IIdentifiableEntity<TKey>, new()
        where UContext : DbContext, new()
    {
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entity);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> ExecuteSql(string sql, List<DbParameter> parms);

        IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms);
    }
}
