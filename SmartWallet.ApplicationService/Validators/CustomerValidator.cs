using SmartWallet.ApplicationService.Dto.Request;
using FluentValidation;

namespace SmartWallet.ApplicationService.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerRequestDto>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .Length(1, 100)
                .WithMessage("Name property must be between 1 and 100 characters")
                .Matches(@"^[\p{L}a-zA-Z', -]+$")
            .WithMessage("Name cannot contain special characters.");
            RuleFor(x => x.Surname)
                .Length(1, 100)
                .WithMessage("Surname property must be between 1 and 100 characters")
                .Matches(@"^[\p{L}a-zA-Z', -]+$")
                .WithMessage("Surname property must be between 1 and 100 characters");
        
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email property can't be empty.")
                .EmailAddress();
        }
    }
}
