using Biblioteca.Api.Resources.Books;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Validators.Books
{
    public class SaveBookResourceValidator : AbstractValidator<SaveBookResource>
    {
        public SaveBookResourceValidator()
        {
            RuleFor(a => a.ISBN)
                .NotEmpty();

            RuleFor(a => a.Title)
               .NotEmpty();

            RuleFor(a => a.Categories)
              .NotEmpty();

            RuleFor(a => a.Authors)
             .NotEmpty();
        }
    }
}
