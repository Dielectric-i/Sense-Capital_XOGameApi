using System.Net;

namespace Sense_Capital_XOGameApi.Common.Errors
{
    public interface IError
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }
    }
}
