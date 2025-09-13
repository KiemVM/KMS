using KMS.Core.ViewModels.Content;
using KMS.Data.Repositories.Content;
using Microsoft.AspNetCore.Mvc;

namespace Order.WebApp.Components
{
    public class GetMenuViewComponent : ViewComponent
    {

        public GetMenuViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
