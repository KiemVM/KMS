using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMS.WebApp.Extensions
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiControllerBase : ControllerBase
    {
    }
}