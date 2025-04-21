using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Kullanicilar
    {
        [Key]
        public int KullaniciID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string KullaniciAdi { get; set; }

        [StringLength(255)]
        public string Sifre { get; set; }

        [StringLength(100)]
        public string Eposta { get; set; }

        [StringLength(255)]
        public string Konum { get; set; }

        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(50)]
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }

        [StringLength(10)]
        public string Cinsiyet { get; set; }

        [StringLength(15)]
        public string TelefonNumarasi { get; set; }

        [StringLength(255)]
        public string ProfilFotografi { get; set; }

        [StringLength(255)]
        public string KullaniciRole { get; set; }
       
        public ICollection<Katilimcilar> katilimcilars { get; set; }
        public ICollection<Mesajlar> gonderilenMesajlars { get; set; }
        public ICollection<Mesajlar> alinanMesajlars { get; set; }
        public ICollection<Puanlar> puanlars { get; set; }
        public ICollection<KullaniciIlgiAlanlari> kullaniciIlgiAlanlaris { get; set; }
        public ICollection<Etkinlikler> etkinliklers { get; set; }
        public virtual ICollection<EtkinlikMesaj> EtkinlikGonderilenMesajlars { get; set; }
        public ICollection<Bildirimler> Bildirimler { get; set; }
    }
}
