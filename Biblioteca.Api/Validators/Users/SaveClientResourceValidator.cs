using Biblioteca.Api.Resources.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Validators.Users
{
    public class SaveClientResourceValidator : AbstractValidator<SaveClientResource>
    {
        public SaveClientResourceValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty();

            RuleFor(a => a.NIF)
               .NotEmpty();

            RuleFor(a => a.Email)
              .NotEmpty();
        }
    }
}
