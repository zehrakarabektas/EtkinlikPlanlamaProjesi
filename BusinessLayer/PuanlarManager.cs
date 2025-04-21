using BusinessLayer.Abstract;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PuanlarManager : IPuanlarService
    {
        IPuanlarDal _puanlarDal;

        public PuanlarManager(IPuanlarDal puanlarDal)
        {
            _puanlarDal=puanlarDal;
        }

        public List<Puanlar> GetAll()
        {
            return _puanlarDal.GetAll();
        }

        public void PuanEkle(Puanlar puan)
        {
            _puanlarDal.Ekle(puan);
        }

        public void PuanGuncelle(Puanlar puan)
        {
            _puanlarDal.Guncelle(puan);
        }

        public void PuanSil(Puanlar puan)
        {
            _puanlarDal.Sil(puan);
        }
        public float ToplamPuanHesapla(int kullaniciID)
        {
           
            var puanlar = _puanlarDal.GetAll()
                .Where(p => p.KullaniciID == kullaniciID)
                .ToList();

            float toplamPuan = 0;

            foreach (var puan in puanlar)
            {
                if(puan.PuanTuru==PuanTuru.Bonus)
                {
                    toplamPuan+=20;
                }
                else if (puan.PuanTuru==PuanTuru.Olusturma)
                {
                    toplamPuan+=15;
                }
                else
                {
                    toplamPuan+=10;
                }

            }
            return toplamPuan;
        }

    }
}
