using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public enum OnayDurumu
    {
        OnayliDegil = 0,  
        Onayli = 1,       
        OnayBekliyor = 2  
    }
    public class Etkinlikler
    {
        [Key]
        public int EtkinlikID { get; set; }

        [StringLength(255)]
        public string EtkinlikAdi { get; set; }
        public string Aciklama { get; set; }
        public DateTime EtkinlikTarih { get; set; }
        public TimeSpan EtkinlikSaati { get; set; }
        public int EtkinlikSuresi { get; set; }
        public OnayDurumu EtkinlikOnayDurumu { get; set; }

        [StringLength(255)]
        public string EtkinlikKonumu { get; set; }

        public string EtkinlikResmi {  get; set; }
        
        public int? IlgiAlaniID { get; set; } 
        public IlgiAlanlari IlgiAlani { get; set; } 

        public int? KullaniciID { get; set; }
        
        public virtual Kullanicilar OlusturanKullanici{ get; set; }

        public virtual ICollection<Katilimcilar> katilimcilars { get; set; }
        public virtual ICollection<EtkinlikMesaj> GonderilenMesajlar { get; set; }

    }
}
