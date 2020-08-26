using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsGatheringServiceMVC.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<Category> _categories;
        public MenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categories = _unitOfWork.CategoryRepository.GetAllAsync().Result;
        }
        public IViewComponentResult Invoke()
        {
            return View("Menu", _categories);
        }
    }
}
