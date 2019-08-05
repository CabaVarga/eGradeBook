using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace eGradeBook.Repositories
{
    /// <summary>
    /// The magical Generic Repository class.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Contructor. We are not accessing any of these directly, everything is going through the UnitOfWork and the Dependency Injector
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// The Get by Id method, using Find (giving me headaches with inheritance)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            try
            {
                logger.Trace("{type} repository Get entity by Id {id}", typeof(TEntity).FullName, id);
                return dbSet.Find(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Get by Id for Id {id} failed", id);
                throw ex;
            }
        }

        /// <summary>
        /// This is the C in CRUD
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            logger.Trace("{type} repository Insert new entity", typeof(TEntity).FullName);

            dbSet.Add(entity);
        }

        /// <summary>
        /// The D in CRUD
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            logger.Trace("{type} repository Delete Id {id}", typeof(TEntity).FullName, id);

            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// A very powerful retrieval method, supporting complex filtering and much more
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            // logger.Trace("{type} repository Get request with filter {filter}, orderBy {orderBy} and include {includeProperties}", 
            logger.Trace("{type} repository Get request with filter and all", typeof(TEntity).FullName);

            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }

            catch (SqlException ex)
            {
                Debug.WriteLine("Database cannot be accessed.");
                throw ex;
            }

            catch (EntityException ex)
            {
                Debug.WriteLine("Database cannot be accessed.");
                throw ex;
            }
        }

        /// <summary>
        /// The D in CRUD, another approach
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// The U in CRUD
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}