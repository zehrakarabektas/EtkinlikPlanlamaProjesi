using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class IlgiAlanlariValidator: AbstractValidator<IlgiAlanlari>
    {
        public IlgiAlanlariValidator()
        {

            RuleFor(x => x.IlgiAlaniAdi).NotEmpty().WithMessage("Ilgi Alanı Adı boş bırakılamaz.");
            RuleFor(x => x.IlgiAlaniAdi).MaximumLength(255).WithMessage("Ilgi Alanı Adı en fazla 255 karakter olabilir.");
            RuleFor(x => x.KategoriID).NotEmpty().WithMessage("Kategori seçilmelidir.");
        }
    }
}
