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

        //public IEnumerable<Category> Categories;
        public IQueryable<Category> Categories;

        public MenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetCategory();
            //_categories = _unitOfWork.CategoryRepository.GetAllAsync().Result;
        }
        private void GetCategory()
        //private async Task GetCategory()
        {
            //Categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            Categories = _unitOfWork.CategoryRepository.GetAllAsync(c => c.Subcategories);//.Result;
            var list = Categories.Select(c => c.Name).ToList();

        }
        public IViewComponentResult Invoke()
        {
            
            return View("Menu", Categories);
        }
    }

}
