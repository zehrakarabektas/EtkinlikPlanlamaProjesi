using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class KatilimcilarValidator : AbstractValidator<Katilimcilar>
    {
        public KatilimcilarValidator()
        {
            RuleFor(x => x.KullaniciID).NotEmpty().WithMessage("Kullanıcı ID boş bırakılamaz.");

            RuleFor(x => x.EtkinlikID).NotEmpty().WithMessage("Etkinlik ID boş bırakılamaz.");
        }
    }
}
