using System.Net;

namespace Sense_Capital_XOGameApi.Common.Errors
{
    public class RequestNotValidException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

        public string ErrorMessage => "Invalid data in the request";
    }
}
