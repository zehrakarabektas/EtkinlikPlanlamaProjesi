using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class EtkinlikKategoriValidator : AbstractValidator<EtkinlikKategori>
    {
        public EtkinlikKategoriValidator()
        {
            //RuleFor(x => x.KategoriAdi)
            //        .NotEmpty().WithMessage("Kategori Adı boş bırakılamaz.")
            //        .MaximumLength(255).WithMessage("Kategori Adı en fazla 255 karakter olabilir.");

            //RuleFor(x => x.KategoriDurumu)
            //        .NotNull().WithMessage("Kategori Durumu belirlenmelidir.");
        }
        
    }
}
