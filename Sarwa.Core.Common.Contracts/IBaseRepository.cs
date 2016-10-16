using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Sarwa.Core.Common.Contracts
{
    public interface IBaseRepository
    {
    }

    public interface IBaseRepository<TEntity, UContext, TKey> : IBaseRepository
        where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entity);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        TEntity ExecuteSql(string sql, List<DbParameter> parms);

        IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms);
    }
}
