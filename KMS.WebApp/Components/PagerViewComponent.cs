using Microsoft.AspNetCore.Mvc;
using KMS.Core.ViewModels;

namespace KMS.WebApp.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PagedResultBase result)
        {
            return View(result);
        }
    }
}
