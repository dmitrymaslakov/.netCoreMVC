using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsCollector.Abstract;
using NewsGatheringService.Core.Abstract;

namespace NewsGatheringServiceMVC.Controllers
{
    public class NewsParserController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUnitOfWork _unitOfWork;

        public NewsParserController(INewsService newsService, IUnitOfWork unitOfWork)
        {
            _newsService = newsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Index()
        //public string Index()
        {
<<<<<<< HEAD
            await 
            _unitOfWork.NewsStructureRepository.DeleteRange(_unitOfWork.NewsStructureRepository.GetAllAsync(n => n.News)//.Result
=======
            await _unitOfWork.NewsStructureRepository.DeleteRange(_unitOfWork.NewsStructureRepository.GetAllAsync()//.Result
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d
                .Select(ns => ns.Id));

            await 
            _unitOfWork.SaveChangesAsync();

<<<<<<< HEAD
            await
            _unitOfWork.NewsRepository.DeleteRange(_unitOfWork.NewsRepository.GetAllAsync()//.Result
            .Select(n => n.Id));
=======
            await _unitOfWork.NewsRepository.DeleteRange(_unitOfWork.NewsRepository.GetAllAsync()//.Result
                .Select(n => n.Id));
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d

            await 
            _unitOfWork.SaveChangesAsync();

<<<<<<< HEAD
            await 
            _unitOfWork.SubcategoryRepository.DeleteRange(_unitOfWork.SubcategoryRepository.GetAllAsync()//.Result
            .Select(s => s.Id));
=======
            await _unitOfWork.SubcategoryRepository.DeleteRange(_unitOfWork.SubcategoryRepository.GetAllAsync()//.Result
                .Select(s => s.Id));
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d

            await 
            _unitOfWork.SaveChangesAsync();

<<<<<<< HEAD
            await
            _unitOfWork.CategoryRepository.DeleteRange(_unitOfWork.CategoryRepository.GetAllAsync()//.Result
            .Select(c => c.Id));
=======
            await _unitOfWork.CategoryRepository.DeleteRange(_unitOfWork.CategoryRepository.GetAllAsync()//.Result
                .Select(c => c.Id));
>>>>>>> b30352d04517ebb7143bbd20c83db4458751190d

            await
            _unitOfWork.SaveChangesAsync();

            //await _newsService.GetDataFromRssAndInsertIntoDb();



            return "done";
        }
    }
}
