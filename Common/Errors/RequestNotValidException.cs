using System.Net;

namespace Sense_Capital_XOGameApi.Common.Errors
{
    public class CreateGameServiceException : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage { get; }
        public CreateGameServiceException(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
