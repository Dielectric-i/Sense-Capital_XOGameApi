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
                IError serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
                
                _ => (StatusCodes.Status500InternalServerError, "Oops, scode500"),
            };
            return Problem(statusCode: statusCode, title: message);
        }
    }
}
