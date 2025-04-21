using BusinessLayer.Abstract;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer
{
    public class KullanicilarManager : IKullanicilarService
    {
        IKullanicilarDal _kullaniciDal;

        public KullanicilarManager(IKullanicilarDal kullaniciDal)
        {
            _kullaniciDal = kullaniciDal;
        }
        public List<Kullanicilar> GetAll()
        {
            return _kullaniciDal.GetAll();
        }

        public void KullaniciEkle(Kullanicilar kullanici)
        {
            _kullaniciDal.Ekle(kullanici);
        }

        public void KullaniciGuncelle(Kullanicilar kullanici)
        {
            _kullaniciDal.Guncelle(kullanici);
        }

        public void KullaniciSil(Kullanicilar kullanici)
        {
            _kullaniciDal.Sil(kullanici);
        }
        public Kullanicilar GetKullaniciID(int kullaniciID)
        {
            return _kullaniciDal.Get(X=>X.KullaniciID==kullaniciID);
        }
        




        //GenericRepository<Kullanicilar> repository = new GenericRepository<Kullanicilar>();

        //public List<Kullanicilar> GetAll()
        //{
        //    return repository.GetAll().ToList();
        //}
        //public void KullaniciEkle(Kullanicilar kullanici)
        //{
        //    repository.Ekle(kullanici);
        //}

    }
}
