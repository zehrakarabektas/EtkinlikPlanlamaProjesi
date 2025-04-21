using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IEtkinlikMesajService
    {
        List<EtkinlikMesaj> GetAll();
        void EtkinlikMesajEkle(EtkinlikMesaj etkinlikMesaj);
        void EtkinlikMesajGuncelle(EtkinlikMesaj etkinlikMesaj);
        void EtkinlikMesajSil(EtkinlikMesaj etkinlikMesaj);
        List<EtkinlikMesaj> GetEtkinlikID(int etkinlikID);
    }
}
