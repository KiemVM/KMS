using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMS.WebApp.Pages
{
    [AllowAnonymous]
    public class Error404Model : PageModel
    {
        public void OnGet()
        {
        }
    }
} 