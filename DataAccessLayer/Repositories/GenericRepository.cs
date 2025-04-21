using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IRepositoryDal<T> where T : class
    {
        public void Ekle(T item)
        {
            using (var context = new Context()) 
            {
                var eklenecekEntity = context.Entry(item);
                eklenecekEntity.State = EntityState.Added;
                context.SaveChanges();
            } 
        }

        public List<T> GetAll()
        {
            using (var context = new Context()) 
            {
                return context.Set<T>().ToList();
            }
        }

        public List<T> GetAll(Expression<Func<T, bool>> filtre)
        {
            using (var context = new Context()) 
            {
                return context.Set<T>().Where(filtre).ToList();
            }
        }

        public void Guncelle(T item)
        {
            using (var context = new Context()) 
            {
                var guncellenecekEntity = context.Entry(item);
                guncellenecekEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Sil(T item)
        {
            using (var context = new Context()) 
            {
                var silinecekEntity = context.Entry(item);
                silinecekEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public T Get(Expression<Func<T, bool>> filtre)
        {
            using (var context = new Context()) 
            {
                return context.Set<T>().SingleOrDefault(filtre);
            }
        }
    }

}

