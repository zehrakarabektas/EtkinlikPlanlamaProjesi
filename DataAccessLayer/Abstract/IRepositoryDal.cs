using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepositoryDal<T>
    {
        List<T> GetAll();
        void Ekle(T item);
        void Sil(T item);
        void Guncelle(T item);  
        List<T> GetAll(Expression<Func<T, bool>> filtre);
        T Get(Expression<Func<T, bool>> filtre);


    }
}
