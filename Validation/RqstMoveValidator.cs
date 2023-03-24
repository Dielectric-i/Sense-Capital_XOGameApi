using FluentValidation;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;
using System.Data;

namespace Sense_Capital_XOGameApi.Validation
{
    public class RqstMoveValidator : AbstractValidator<RqstMakeMove>
    {
        public RqstMoveValidator()
        {
            RuleFor(m => m.Row).ExclusiveBetween(0, 2);
            RuleFor(m=> m.Column).ExclusiveBetween(0, 2);
            RuleFor(m => m.PlayerId).GreaterThan(0);
            RuleFor(m => m.GameId).GreaterThan(0);
        }
    }
}
