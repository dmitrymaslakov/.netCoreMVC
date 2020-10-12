using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringService.UOW.DAL.Interfaces;

namespace NewsGatheringService.MVC.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        const string ERROR_MESSAGE = "User already exists";

        public AccountController(IUnitOfWork unitOfWork, ILogger<AccountController> logger, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = await _userService.RegisterUserAsync(model);
                    
                    if (userId != null)
                    {
                        var claimsPrincipalId = _userService.AuthenticateWithCookie(new AuthenticateRequest { Login = model.Login, Password = model.Password });
                        
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipalId));

                        return RedirectToAction("Index", "Home");
                    }
                    
                    else
                        ModelState.AddModelError("", ERROR_MESSAGE);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            try
            {
                if (_unitOfWork.UserRepository == null || _unitOfWork.UserRepository.FindBy(u => u.Login.Contains("admin")).Count() == 0)
                    await _userService.RegisterAdminAsync();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return BadRequest();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var claimsPrincipalId = _userService.AuthenticateWithCookie(model);
                    
                    if (claimsPrincipalId != null)
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipalId));
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Username or password is incorrect");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return BadRequest();
            }

        }
        
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return BadRequest();
            }
        }
    }
}
