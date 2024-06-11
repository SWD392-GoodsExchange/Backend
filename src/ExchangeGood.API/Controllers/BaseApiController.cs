using ExchangeGood.API.Middleware;
using Microsoft.AspNetCore.Mvc;
namespace ExchangeGood.API.Controllers {

    [ServiceFilter(typeof(ExecuteValidation))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase {
    }
}
