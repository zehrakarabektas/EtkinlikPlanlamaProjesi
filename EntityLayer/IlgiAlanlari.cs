using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
 
    public class IlgiAlanlari
    {
        [Key]
        public int IlgiAlaniID { get; set; }

        [StringLength(255)]
        public string IlgiAlaniAdi { get; set; }

        public int KategoriID { get; set; }
        public OnayDurumu OnaylanmaDurumu {  get; set; }
        public virtual EtkinlikKategori Kategori { get; set; }

        public ICollection<KullaniciIlgiAlanlari> kullaniciIlgiAlanlaris { get; set; }
        public ICollection<Etkinlikler> Etkinliklers { get; set; }
    }
}
