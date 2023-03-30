using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sense_Capital_XOGameApi.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Errors()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            return Problem(title: exception?.Message, statusCode:400) ;
        }
    }
}
