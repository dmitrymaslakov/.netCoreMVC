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
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Interfaces;
using NewsGatheringService.Models.BLL;


namespace NewsGatheringService.MVC.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        const string errorMessage = "User already exists";

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
                    User user = _unitOfWork.UserRepository?
                        .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                        .FirstOrDefault();

                    if (user == null)
                    {
                        /*user = new User
                        {
                            Id = Guid.NewGuid(),
                            Login = model.Login,
                        };

                        var passwordToBytes = Encoding.UTF8.GetBytes(model.Password);
                        var enText = Convert.ToBase64String(passwordToBytes);

                        user.PasswordHash = enText;

                        Role userRole = _unitOfWork.RoleRepository.FindBy(r => r.Name == "user").FirstOrDefault();
                        if (userRole != null)
                            user.UserRoles =
                                user.UserRoles
                                .Append(new UserRole
                                { Id = Guid.NewGuid(), RoleId = userRole.Id, UserId = user.Id, Date = DateTime.Now }).ToList();

                        await _unitOfWork.UserRepository.AddAsync(user);

                        await _unitOfWork.SaveChangesAsync();*/
                        var userId = await _userService.RegisterUserAsync(model);
                        /*user = user = _unitOfWork.UserRepository?
                            .FindBy(u => u.Id.CompareTo(userId) == 0, u => u.UserRoles)
                            .FirstOrDefault();*/

                        //await AuthenticateWithCookie(user);
                        var claimsPrincipalId = _userService.AuthenticateWithCookie(new AuthenticateRequest { Login = model.Login, Password = model.Password });
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipalId));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", errorMessage);
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

                //if (_unitOfWork.RoleRepository == null || _unitOfWork.RoleRepository.FindBy(r => r.Name.Equals("admin")).Count() == 0)
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
        //public async Task<IActionResult> Login(LoginModel model)
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    /*User user = _unitOfWork.UserRepository
                        .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        //await AuthenticateWithCookie(user);
                        var claimsPrincipalId = _userService.AuthenticateWithCookie(user);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipalId));

                        return RedirectToAction("Index", "Home");
                    }*/
                    var claimsPrincipalId = _userService.AuthenticateWithCookie(model);
                    if (claimsPrincipalId != null)
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipalId));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
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
        /*private async Task AuthenticateWithCookie(User user)
        {
            var role = _unitOfWork.UserRoleRepository
                .FindBy(ur => ur.UserId.CompareTo(user.Id) == 0, ur => ur.Role)
                .FirstOrDefault()?
                .Role;

            var login = user.Login;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }*/

        /*private async Task InitializeAdminUserAsync()
        {
            const string adminRoleName = "admin";
            const string userRoleName = "user";

            var adminLogin = "adminLogin";
            var adminPassword = "123456";


            var adminRole = new Role { Id = Guid.NewGuid(), Name = adminRoleName };
            var userRole = new Role { Id = Guid.NewGuid(), Name = userRoleName };
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Login = adminLogin,
                PasswordHash = BC.HashPassword(adminPassword)
        };

            adminUser.UserRoles = adminUser.UserRoles
            .Append(new UserRole
            { Id = Guid.NewGuid(), RoleId = adminRole.Id, UserId = adminUser.Id, Date = DateTime.Now }).ToList();


            await _unitOfWork.RoleRepository.AddRangeAsync(new List<Role> { adminRole, userRole });

            await _unitOfWork.UserRepository.AddAsync(adminUser);

            await _unitOfWork.SaveChangesAsync();
        }*/
    }
}
