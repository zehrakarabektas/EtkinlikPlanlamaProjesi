using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPuanlarService
    {
        List<Puanlar> GetAll();
        void PuanEkle(Puanlar puan);
        void PuanGuncelle(Puanlar puan);
        void PuanSil(Puanlar puan);


    }
}
