using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IKullanicilarService
    {
        List<Kullanicilar> GetAll();
        void KullaniciEkle(Kullanicilar kullanici);
        void KullaniciGuncelle(Kullanicilar kullanici); 
        void KullaniciSil(Kullanicilar kullanici);
        Kullanicilar GetKullaniciID(int kullaniciID);

    }
}
