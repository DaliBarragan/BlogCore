using BlogCore.AccesoDatos.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            dbSet = context.Set<T>();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //Se crea una consulta IQueryable a partor del dbSet del contexto
            IQueryable<T> query = dbSet;

            //Se aplica el filtro si se pasa
            if (filter != null)
            {
                //Si se pasa un filtro, se aplica a la consulta
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //Si se pasan propiedades a incluir, se separan por comas y se incluyen en la consulta
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                //Si se pasa un orden, se aplica a la consulta
                return orderBy(query).ToList();
            }
            
            //Si no se pasa un orden, se devuelve la consulta sin ordenar
            return query.ToList();

        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            //Se aplica el filtro si se pasa
            if (filter != null)
            {
                //Si se pasa un filtro, se aplica a la consulta
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //Si se pasan propiedades a incluir, se separan por comas y se incluyen en la consulta
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            
            return query.FirstOrDefault();

        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}