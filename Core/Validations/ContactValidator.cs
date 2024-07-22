using Core.Entities;
using FluentValidation;

namespace Core.Validations
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(contact => contact.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser válido.");

            RuleFor(contact => contact.Ddd)
                .NotEmpty().WithMessage("O DDD é obrigatório.")
                .Matches("^\\d{2}$").WithMessage("O DDD deve ter dois dígitos numéricos.")
                .GreaterThanOrEqualTo("11").WithMessage("O DDD deve ser 11 ou maior.");

            RuleFor(contact => contact.Name)
                .NotEmpty().WithMessage("O Nome é obrigatório.");

            RuleFor(contact => contact.Phone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches("^\\d{8,9}$").WithMessage("Informe somente números no telefone sem DDD");
        }
    }
}
