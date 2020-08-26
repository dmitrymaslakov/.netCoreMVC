using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Components
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
