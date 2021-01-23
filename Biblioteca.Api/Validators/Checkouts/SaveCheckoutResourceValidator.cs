using Biblioteca.Api.Resources.Checkouts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Validators.Checkouts
{
    public class SaveCheckoutResourceValidator : AbstractValidator<SaveCheckoutResource>
    {

        public SaveCheckoutResourceValidator()
        {
            //RuleFor(a => a.ClientId)
            //    .NotEmpty();

            RuleFor(a => a.Date)
               .NotEmpty();

            RuleFor(a => a.CheckoutBooks)
              .NotEmpty();
        }
    }
}
