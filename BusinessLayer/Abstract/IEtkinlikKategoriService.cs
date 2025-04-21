using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IEtkinlikKategoriService
    {
        List<EtkinlikKategori> GetAll();
        void EtkinlikKategoriEkle(EtkinlikKategori etkinlikKategori);
        void EtkinlikKategoriGuncelle(EtkinlikKategori etkinlikKategori); 
        void EtkinlikKategoriSil(EtkinlikKategori etkinlikKategori);  
        EtkinlikKategori GetKategoriID(int kategoriID);
    }
}
