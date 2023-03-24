using FluentValidation;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Validation
{
    public class RqstCreateGameValidator: AbstractValidator<RqstCreateGame>
    {
        public RqstCreateGameValidator()
        {
            RuleFor(rqst => rqst.P1Name).MaximumLength(50).MinimumLength(3);
            RuleFor(rqst => rqst.P2Name).MaximumLength(50).MinimumLength(3);
        }
    }
}
