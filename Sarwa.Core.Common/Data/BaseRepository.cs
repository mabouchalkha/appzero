using Sarwa.Core.Common.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Sarwa.Core.Common.Data
{
    public abstract class BaseRepository<UContext, TEntity, TKey> : IBaseRepository<TEntity, UContext, TKey>
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

            // IQueryable<TEntity> query = DbSet(entityContext);
            //foreach (var includeProperty in includeProperties)
            //{
            //    query = query.Include(includeProperty);
            //}
            //return query;
        }

        private TEntity GetEntity(UContext entityContext, TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetEntities(entityContext, includeProperties).SingleOrDefault(IdentifierPredicate(entityContext, id));
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllQueryable(includeProperties).ToList(); //.AsEnumerable();
        }

        public IQueryable<TEntity> GetAllQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
            {
                return GetEntities(entityContext, includeProperties);
            }
        }

        public TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
            {
                return GetEntity(entityContext, id, includeProperties);
            }
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetByQueryable(predicate, includeProperties).AsEnumerable();
        }

        public IQueryable<TEntity> GetByQueryable(Expression<Func<TEntity, bool>> predicate, 
                                             params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
            {
                return GetEntities(entityContext, includeProperties).Where(predicate);
            }
        }

        public TEntity Add(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity addedEntity = DbSet(entityContext).Add(entity);

                entityContext.SaveChanges();

                return addedEntity;
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                // entityContext.Set<TEntity>().Attach(entity);
                entityContext.Entry(entity).State = EntityState.Modified;

                entityContext.SaveChanges();

                return entity;
            }
        }

        public void Delete(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }
   
        public void Delete(TKey id)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity entity = GetEntity(entityContext, id);
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
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

        public int Count()
        {
            using (UContext entityContext = new UContext())
            {
                return GetEntities(entityContext).Count();
            }
        }

        public TEntity ExecuteSql(string sql, List<DbParameter> parms)
        {
            using (UContext entityContext = new UContext())
            {
                DbDataReader reader = (DbDataReader)(ExecuteSql(entityContext, sql, parms));
                ObjectResult<TEntity> result = ((IObjectContextAdapter)entityContext).ObjectContext.Translate<TEntity>(reader);
                return result.FirstOrDefault();
            }
        }

        public IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms)
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
}
