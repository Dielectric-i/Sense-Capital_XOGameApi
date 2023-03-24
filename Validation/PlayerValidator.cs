using FluentValidation;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Validation
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(Player => Player.Name).MaximumLength(50).MinimumLength(3);
        }
    }
}