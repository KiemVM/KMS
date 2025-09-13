using Microsoft.AspNetCore.Mvc;

namespace KMS.WebApp.Components
{
    public class GetSidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
