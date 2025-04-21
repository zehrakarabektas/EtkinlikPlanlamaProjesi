using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public enum BildirimDurumu
    {
        Okunmadı = 0,
        Okundu = 1,
        Silindi = 2
    }
    public class Bildirimler
    {
        [Key]
        public int BildirimID { get; set; }
        public int MesajID { get; set; }  
        public int KullaniciID { get; set; } 
        public string Mesaj { get; set; }  
        public BildirimDurumu Durum { get; set; }
        public DateTime Tarih { get; set; }

        public virtual EtkinlikMesaj EtkinlikMesaj { get; set; }
        public virtual Kullanicilar Kullanici { get; set; }
    }
}
