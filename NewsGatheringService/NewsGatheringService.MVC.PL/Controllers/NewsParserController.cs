using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.DAL.Interfaces;

namespace NewsGatheringService.MVC.PL.Controllers
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

        public async Task<string> DeleteNews()
        {
            await _unitOfWork.NewsStructureRepository.DeleteRange(
                _unitOfWork
                .NewsStructureRepository
                .GetAllAsync().ToList().AsQueryable()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable()
                );

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.NewsRepository.DeleteRange(
                _unitOfWork.NewsRepository
                .GetAllAsync()
                .Select(n => n.Id)
                .ToList()
                .AsQueryable()
                );

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.SubcategoryRepository.DeleteRange(_unitOfWork.SubcategoryRepository.GetAllAsync()
            .Select(s => s.Id).ToList().AsQueryable());

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CategoryRepository.DeleteRange(_unitOfWork.CategoryRepository.GetAllAsync()
            .Select(c => c.Id).ToList().AsQueryable());

            await _unitOfWork.SaveChangesAsync();

            //await _newsService.GetDataFromRssAndInsertIntoDb();

            return "done";
        }
        public async Task<string> DeleteUsers()
        {
            await _unitOfWork.UserRoleRepository.DeleteRange(_unitOfWork
                .UserRoleRepository
                .GetAllAsync()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.RefreshTokenRepository.DeleteRange(_unitOfWork
                .RefreshTokenRepository
                .GetAllAsync()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.UserRepository.DeleteRange(_unitOfWork
                .UserRepository
                .GetAllAsync()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            return "done";
        }
        public async Task<string> DeleteRoles()
        {
            await _unitOfWork.UserRoleRepository.DeleteRange(_unitOfWork
                .UserRoleRepository
                .GetAllAsync()
                .Select(ur => ur.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.RoleRepository.DeleteRange(_unitOfWork
                .RoleRepository
                .GetAllAsync()
                .Select(r => r.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            return "done";
        }

    }
}
