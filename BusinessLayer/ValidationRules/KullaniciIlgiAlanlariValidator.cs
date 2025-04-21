using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class KullaniciIlgiAlanlariValidator : AbstractValidator<KullaniciIlgiAlanlari>
    {
        public KullaniciIlgiAlanlariValidator()
        {
            RuleFor(x => x.KullaniciID).NotEmpty().WithMessage("Kullanıcı ID boş bırakılamaz.");

            RuleFor(x => x.IlgiAlaniID).NotEmpty().WithMessage("İlgi Alanı ID boş bırakılamaz.");
        }
    }
}
