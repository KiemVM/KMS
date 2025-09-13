using Microsoft.AspNetCore.Mvc;
using KMS.Common.Models;

namespace KMS.WebApp.Components
{
    public class ButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ButtonViewModel buttonViewModel)
        {
            return View(buttonViewModel);
        }
    }
}
