using FluentValidation;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Validation
{
    public class GameInitialStateValidator : AbstractValidator<Game>
    {
        public GameInitialStateValidator()
        {
            RuleFor(g => g.Player1Symbol).Equal("X");
            RuleFor(g => g.Player2Symbol).Equal("O");
            RuleFor(g => g.WinnerId).Null();
            RuleFor(g => g.BoardState).Equal("---------");
            RuleFor(g => g.CurrentPlayerId).Equal(g => g.Players[0].Id);
            RuleForEach(g => g.Players).SetValidator(new PlayerValidator());
        }
    }
}
