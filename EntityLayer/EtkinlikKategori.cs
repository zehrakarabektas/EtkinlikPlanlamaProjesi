using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class EtkinlikKategori
    {
        [Key]
        public int KategoriID { get; set; }

        [StringLength(255)]
        public string KategoriAdi { get; set; }
        public string KategoriResmi {  get; set; }
        public bool KategoriOnayDurumu { get; set; }
        public virtual ICollection<IlgiAlanlari> IlgiAlanlaris { get; set; }

    }
}
