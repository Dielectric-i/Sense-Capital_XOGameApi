using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Common.Errors;

namespace Sense_Capital_XOGameApi.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Errors()
        {

            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;




            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
                //RequestNotValidException _ => (StatusCodes.Status422UnprocessableEntity , "422 message"),
                _ => (StatusCodes.Status500InternalServerError, "Oops, scode500"),
            };
            //return Problem(statusCode: statusCode, title:message);
            return Problem(statusCode: statusCode, title: message);
        }
    }
}
