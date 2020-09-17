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

<<<<<<< HEAD
        //public IEnumerable<Category> Categories;
        public IQueryable<Category> Categories;
=======
        public IEnumerable<Category> Categories;
        //public IQueryable<Category> Categories;
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d

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
<<<<<<< HEAD
            Categories = _unitOfWork.CategoryRepository.GetAllAsync(c => c.Subcategories);//.Result;
=======
            Categories = _unitOfWork.CategoryRepository.GetAllAsync();//.Result;
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d
            var list = Categories.Select(c => c.Name).ToList();

        }
        public IViewComponentResult Invoke()
        {
            
            return View("Menu", Categories);
        }
    }

}
