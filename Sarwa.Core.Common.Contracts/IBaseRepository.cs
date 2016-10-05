using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sarwa.Core.Common.Contracts
{
    public interface IBaseRepository
    {
    }

    public interface IBaseRepository<TEntity, UContext, TKey> : IBaseRepository
        where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetAllQueryable(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetByQueryable(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        int Count();

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entity);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        TEntity ExecuteSql(string sql, List<DbParameter> parms);

        IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms);
    }
}
