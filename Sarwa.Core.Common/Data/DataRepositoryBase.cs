using Sarwa.Core.Common.Contracts;
using Sarwa.Core.Common.Helpers;
using Sarwa.Core.Common.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Sarwa.Core.Common.Data
{
    public abstract class DataRepositoryBase<TEntity, UContext, TKey> : IDataRepositoryBase<TEntity, UContext, TKey>
        where TEntity : class, IIdentifiableEntity<TKey>, new()
        where UContext : DbContext, new()
    {
        protected abstract Expression<Func<TEntity, bool>> IdentifierPredicate(UContext entityContext, TKey id);
        protected abstract DbSet<TEntity> DbSet(UContext entityContext);

        private IQueryable<TEntity> GetEntities(UContext entityContext, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet(entityContext).AsNoTracking();

            return includeProperties
                        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private TEntity GetEntity(UContext entityContext, TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetEntities(entityContext, includeProperties).SingleOrDefault(IdentifierPredicate(entityContext, id));
        }

        public virtual IPagedList<TEntity> Query(string sort, int page, int pageSize, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
            {
                var entities = GetEntities(entityContext, includeProperties).ApplySort(sort);
                return new PagedList<TEntity>(entities, page, pageSize);
            }
        }
        public virtual IEnumerable<TEntity> GetAll(string sort = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntities(entityContext, includeProperties).ApplySort(sort).ToList();
            //DbFunctions.
        }

        public virtual TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntity(entityContext, id, includeProperties);
        }

        public virtual IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntities(entityContext, includeProperties).Where(predicate).ToList();
        }

        public virtual TEntity Add(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity addedEntity = DbSet(entityContext).Add(entity);

                entityContext.SaveChanges();

                return addedEntity;
            }
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            using (UContext entityContext = new UContext())
            {
                IEnumerable<TEntity> addedEntities = DbSet(entityContext).AddRange(entities);

                entityContext.SaveChanges();

                return addedEntities;
            }
        }

        public virtual TEntity Update(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity existingEntity = GetEntity(entityContext, entity.EntityId);

                if (existingEntity == null)
                    throw new InvalidOperationException();

                entityContext.Entry(entity).State = EntityState.Modified;

                entityContext.SaveChanges();

                return entity;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public virtual void Delete(TKey id)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity entity = GetEntity(entityContext, id);
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            using (UContext entityContext = new UContext())
            {
                IQueryable<TEntity> entities = GetEntities(entityContext).Where(predicate);

                foreach (var entity in entities)
                {
                    entityContext.Entry(entity).State = EntityState.Deleted;
                }

                entityContext.SaveChanges();
            }
        }

        public virtual TEntity ExecuteSql(string sql, List<DbParameter> parms)
        {
            using (UContext entityContext = new UContext())
            {
                DbDataReader reader = (DbDataReader)(ExecuteSql(entityContext, sql, parms));
                ObjectResult<TEntity> result = ((IObjectContextAdapter)entityContext).ObjectContext.Translate<TEntity>(reader);
                return result.FirstOrDefault();
            }
        }

        public virtual IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms)
        {
            DbCommand command = entityContext.Database.Connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (parms != null)
                foreach (DbParameter p in parms)
                    command.Parameters.Add(p);

            if (entityContext.Database.Connection.State != ConnectionState.Open)
                entityContext.Database.Connection.Open();

            IDataReader reader = command.ExecuteReader();
            return reader;
        }
    }

    //public class PropertyIncluder<TEntity> where TEntity : class
    //{
    //    private readonly Func<DbQuery<TEntity>, DbQuery<TEntity>> _includeMethod;
    //    private readonly HashSet<Type> _visitedTypes;

    //    public PropertyIncluder()
    //    {
    //        //Recursively get properties to include
    //        _visitedTypes = new HashSet<Type>();
    //        var propsToLoad = GetPropsToLoad(typeof(TEntity)).ToArray();

    //        _includeMethod = d =>
    //        {
    //            var dbSet = d;
    //            foreach (var prop in propsToLoad)
    //            {
    //                dbSet = dbSet.Include(prop);
    //            }

    //            return dbSet;
    //        };
    //    }

    //    private IEnumerable<string> GetPropsToLoad(Type type)
    //    {
    //        _visitedTypes.Add(type);
    //        var propsToLoad = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
    //                                          .Where(p => p.GetCustomAttributes(typeof(IncludeAttribute), true).Any());

    //        foreach (var prop in propsToLoad)
    //        {
    //            yield return prop.Name;

    //            if (_visitedTypes.Contains(prop.PropertyType))
    //                continue;

    //            foreach (var subProp in GetPropsToLoad(prop.PropertyType))
    //            {
    //                yield return prop.Name + "." + subProp;
    //            }
    //        }
    //    }

    //    public DbQuery<TEntity> BuildQuery(DbSet<TEntity> dbSet)
    //    {
    //        return _includeMethod(dbSet);
    //    }
    //}
}
