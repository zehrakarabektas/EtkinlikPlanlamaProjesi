using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EtkinlikMesajManager: IEtkinlikMesajService
    {
        IEtkinlikMesajDal _etkinlikMesajDal;

        public EtkinlikMesajManager(IEtkinlikMesajDal etkinlikMesajDal)
        {
            _etkinlikMesajDal=etkinlikMesajDal;
        }

        public void EtkinlikMesajEkle(EtkinlikMesaj etkinlikMesaj)
        {
            _etkinlikMesajDal.Ekle(etkinlikMesaj);
        }

        public void EtkinlikMesajGuncelle(EtkinlikMesaj etkinlikMesaj)
        {
            _etkinlikMesajDal.Guncelle(etkinlikMesaj);
        }

        public void EtkinlikMesajSil(EtkinlikMesaj etkinlikMesaj)
        {
            _etkinlikMesajDal.Sil(etkinlikMesaj);
        }

        public List<EtkinlikMesaj> GetAll()
        {
            return _etkinlikMesajDal.GetAll();
        }

        public List<EtkinlikMesaj> GetEtkinlikID(int etkinlikID)
        {
            return _etkinlikMesajDal.GetAll().Where(x => x.EtkinlikID==etkinlikID).OrderBy(x => x.GonderimZamani).ToList(); 
        }
    }
}
