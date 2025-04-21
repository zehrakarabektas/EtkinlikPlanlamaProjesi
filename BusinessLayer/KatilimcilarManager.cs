using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class KatilimcilarManager : IKatilimcilarService
    {
        IKatilimcilarDal _katilimcilarDal;

        public KatilimcilarManager(IKatilimcilarDal katilimcilarDal)
        {
            _katilimcilarDal=katilimcilarDal;
        }

        public List<Katilimcilar> GetAll()
        {
            return _katilimcilarDal.GetAll();
        }

        public void KatilimciEkle(Katilimcilar katilimci)
        {
            _katilimcilarDal.Ekle(katilimci);
        }

        public void KatilimciGuncelle(Katilimcilar katilimci)
        {
            _katilimcilarDal.Guncelle(katilimci);
        }

        public void KatilimciSil(Katilimcilar katilimci)
        {
            _katilimcilarDal.Sil(katilimci);
        }
        public List<int> GetKullaniciKatildigiEtkinliklerID(int kullaniciID)
        {
            return _katilimcilarDal.GetAll(x => x.KullaniciID == kullaniciID).Select(x => x.EtkinlikID).ToList();
        }
        public List<int> GetKatilimciList(int etkinlikId)
        {
            return _katilimcilarDal.GetAll(x => x.EtkinlikID == etkinlikId).Select(x=>x.KullaniciID).ToList();
        }


    }
}
