using SmartWallet.ApplicationService.Dto.Request;
using FluentValidation;

namespace SmartWallet.ApplicationService.Validators
{
    public class BalanceHistoryValidator : AbstractValidator<BalanceHistoryRequestDto>
    {
        public BalanceHistoryValidator()
        {
            RuleFor(x => x.Category)
                .Length(1, 100)
                .WithMessage("Category property must be between 1 and 100 characters");
            
            RuleFor(x => x.Description)
                .Length(1, 100)
                .WithMessage("Description property must be between 1 and 100 characters");

            RuleFor(x => x.Variation)
                .GreaterThan(0)
                .WithMessage("Variation property must be a greater than 0.");
        }
    }
}
