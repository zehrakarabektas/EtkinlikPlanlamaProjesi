using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBildirimService
    {
        List<Bildirimler> GetAll();
        void BildiriEkle(Bildirimler bildiri);
        void BildiriGuncelle(Bildirimler bildiri);
        void BildiriSil(Bildirimler bildiri);
        Bildirimler GetBildiriID(int bildiriID);
    }
   
}
