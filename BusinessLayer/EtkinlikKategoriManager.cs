using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EtkinlikKategoriManager: IEtkinlikKategoriService
    {
        IEtkinlikKategoriDal _etkinlikKategoriDal;
        public EtkinlikKategoriManager(IEtkinlikKategoriDal etkinlikKategoriDal)
        {
            _etkinlikKategoriDal = etkinlikKategoriDal;
        }
        public List<EtkinlikKategori> GetAll()
        {
            return _etkinlikKategoriDal.GetAll();
        }
        public void EtkinlikKategoriEkle(EtkinlikKategori etkinlikKategori)
        {
            _etkinlikKategoriDal.Ekle(etkinlikKategori);
        }

        public void EtkinlikKategoriGuncelle(EtkinlikKategori etkinlikKategori)
        {
            _etkinlikKategoriDal.Guncelle(etkinlikKategori);
        }
       
        public EtkinlikKategori GetKategoriID(int kategoriID)
        {
            return _etkinlikKategoriDal.Get(x=>x.KategoriID==kategoriID);
        }

        public void EtkinlikKategoriSil(EtkinlikKategori etkinlikKategori)
        {
            _etkinlikKategoriDal.Sil(etkinlikKategori);
        }
    }
}
