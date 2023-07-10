using SmartWallet.ApplicationService.Dto.Request;
using FluentValidation;

namespace SmartWallet.ApplicationService.Validators
{
    public class CoinValidator : AbstractValidator<CoinRequestDto>
    {
        public CoinValidator()
        {
            RuleFor(x => x.Name)
                .Length(3, 4)
                .WithMessage("Name property must be between 3 and 4 characters");
        }
    }
}
