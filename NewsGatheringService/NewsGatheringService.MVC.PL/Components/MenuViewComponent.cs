using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Interfaces;
using System.Linq;

namespace NewsGatheringService.MVC.PL.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public IQueryable<Category> Categories;

        public MenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetCategory();
        }
        private void GetCategory()
        {
            Categories = _unitOfWork.CategoryRepository.FindBy(null, c => c.Subcategories);
        }
        public IViewComponentResult Invoke()
        {
            return View("Menu", Categories);
        }
    }

}
