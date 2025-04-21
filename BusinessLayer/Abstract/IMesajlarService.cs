using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMesajlarService
    {
        List<Mesajlar> GetAll();
        void MesajEkle(Mesajlar mesaj);
        void MesajGuncelle(Mesajlar mesaj);
        void MesajSil(Mesajlar mesaj);
        Mesajlar GetMesajID(int mesajId);
    }
}
