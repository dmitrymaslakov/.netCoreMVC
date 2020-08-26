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
        {
            await _unitOfWork.NewsStructureRepository.DeleteRange(_unitOfWork.NewsStructureRepository.GetAllAsync().Result.Select(ns => ns.Id));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.NewsRepository.DeleteRange(_unitOfWork.NewsRepository.GetAllAsync().Result.Select(n => n.Id));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.SubcategoryRepository.DeleteRange(_unitOfWork.SubcategoryRepository.GetAllAsync().Result.Select(s => s.Id));

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CategoryRepository.DeleteRange(_unitOfWork.CategoryRepository.GetAllAsync().Result.Select(c => c.Id));

            await _unitOfWork.SaveChangesAsync();

            //await _newsService.GetDataFromRssAndInsertIntoDb();



            return "done";
        }
    }
}
