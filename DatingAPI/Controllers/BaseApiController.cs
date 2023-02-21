using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //can be accessed via GET/api/users URL.
    public class BaseApiController : ControllerBase
    {
        
    }
}