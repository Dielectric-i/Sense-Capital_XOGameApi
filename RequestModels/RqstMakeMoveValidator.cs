using FluentValidation;
using Microsoft.AspNetCore.Connections.Features;

namespace Sense_Capital_XOGameApi.RequestModels
{
    public class RqstMakeMoveValidator : AbstractValidator<RqstMakeMove>
    {
        public RqstMakeMoveValidator()
        {
            RuleFor(rqst => rqst.Row).InclusiveBetween(0, 3);
            RuleFor(rqst => rqst.Column).InclusiveBetween(0, 3);
        }
    }
}
