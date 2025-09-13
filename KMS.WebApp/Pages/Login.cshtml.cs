using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMS.WebApp.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}