using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace KMS.WebApp.Extensions
{
    [Authorize]
    public class PageModelBase : PageModel
    {
    }
}