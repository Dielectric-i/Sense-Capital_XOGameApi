using FluentValidation;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Validation
{
    public class RqstCreatePlayerValidator : AbstractValidator<RqstCreatePlayer>
    {
        public RqstCreatePlayerValidator()
        {
            RuleFor(rqst => rqst.Name).MaximumLength(50).MinimumLength(3);
        }
    }
}