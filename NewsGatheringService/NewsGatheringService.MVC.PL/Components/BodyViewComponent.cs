using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace NewsGatheringService.MVC.PL.Components
{
    public class BodyViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string bodyNews)
        {
            return new HtmlContentViewComponentResult(
                new HtmlString(bodyNews)
            );
        }
    }
}
