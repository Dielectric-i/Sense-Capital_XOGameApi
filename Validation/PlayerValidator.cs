using FluentValidation;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.Validation.Rules;

namespace Sense_Capital_XOGameApi.Validation
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(p => p.Name).ValidateName();
        }
    }
}