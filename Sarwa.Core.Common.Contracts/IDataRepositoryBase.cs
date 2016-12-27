using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Sarwa.Core.Common.Contracts
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

        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entity);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        TEntity ExecuteSql(string sql, List<DbParameter> parms);

        IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms);
    }
}
