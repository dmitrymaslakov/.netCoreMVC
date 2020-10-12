using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsGatheringService.UOW.DAL.Interfaces;

namespace NewsGatheringService.MVC.PL.Controllers
{
    public class NewsParserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsParserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> DeleteNews()
        {
            await _unitOfWork.NewsStructureRepository.DeleteRange(
                _unitOfWork
                .NewsStructureRepository
                .GetAllAsQueryable()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable()
                );

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.NewsUrlRepository.DeleteRange(
                _unitOfWork
                .NewsUrlRepository
                .GetAllAsQueryable()
                .Select(nu => nu.Id)
                .ToList()
                .AsQueryable()
                );

            await _unitOfWork.SaveChangesAsync();


            await _unitOfWork.NewsRepository.DeleteRange(
                _unitOfWork.NewsRepository
                .GetAllAsQueryable()
                .Select(n => n.Id)
                .ToList()
                .AsQueryable()
                );

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.SubcategoryRepository.DeleteRange(_unitOfWork.SubcategoryRepository.GetAllAsQueryable()
            .Select(s => s.Id).ToList().AsQueryable());

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CategoryRepository.DeleteRange(_unitOfWork.CategoryRepository.GetAllAsQueryable()
            .Select(c => c.Id).ToList().AsQueryable());

            await _unitOfWork.SaveChangesAsync();

            //await _newsService.GetDataFromRssAndInsertIntoDb();

            return "done";
        }

        public async Task<string> DeleteUsers()
        {
            await _unitOfWork.UserRoleRepository.DeleteRange(_unitOfWork
                .UserRoleRepository
                .GetAllAsQueryable()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.RefreshTokenRepository.DeleteRange(_unitOfWork
                .RefreshTokenRepository
                .GetAllAsQueryable()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.UserRepository.DeleteRange(_unitOfWork
                .UserRepository
                .GetAllAsQueryable()
                .Select(ns => ns.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            return "done";
        }

        public async Task<string> DeleteUserByLogin(params string[] logins)
        {

            await _unitOfWork.UserRoleRepository.DeleteRange(_unitOfWork
                .UserRoleRepository
                .GetAllAsQueryable()
                .Where(ur => logins.Any(login => login.Equals(ur.User.Login)))
                .Select(ur => ur.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.RefreshTokenRepository.DeleteRange(_unitOfWork
                .RefreshTokenRepository
                .GetAllAsQueryable()
                .Where(t => logins.Any(login => login.Equals(t.User.Login)))
                .Select(t => t.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.UserRepository.DeleteRange(_unitOfWork
                .UserRepository
                .GetAllAsQueryable()
                .Where(u => logins.Any(login => login.Equals(u.Login)))
                .Select(u => u.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            return "done";
        }

        public async Task<string> DeleteRoles()
        {
            await _unitOfWork.UserRoleRepository.DeleteRange(_unitOfWork
                .UserRoleRepository
                .GetAllAsQueryable()
                .Select(ur => ur.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.RoleRepository.DeleteRange(_unitOfWork
                .RoleRepository
                .GetAllAsQueryable()
                .Select(r => r.Id)
                .ToList()
                .AsQueryable());
            await _unitOfWork.SaveChangesAsync();

            return "done";
        }

    }
}
