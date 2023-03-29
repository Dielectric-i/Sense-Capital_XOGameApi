using FluentValidation;

namespace Sense_Capital_XOGameApi.Validation.Rules
{
    public static class PlayerValidations
    {
        public static IRuleBuilder<T, string> ValidateName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(3, 50);
        }
    }
}
