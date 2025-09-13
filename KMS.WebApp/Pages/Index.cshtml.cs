using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMS.WebApp.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;


        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(Guid? tableId = null)
        {
            try
            {

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading homepage: {ex.Message}");
                return Page();
            }
        }
    }
}