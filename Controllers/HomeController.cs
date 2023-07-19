using EmployeeControlService.Entities;
using EmployeePerformanceReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Transactions;

namespace EmployeePerformanceReview.Controllers
{
    public class HomeController : Controller
    {
        private readonly Db _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            Db db,
            ILogger<HomeController> logger
            )
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult UserTable(UserInfo user)
        {
            return View(user);
        }

        public IActionResult AdminTable(UserInfo admin)
        {
            return View(admin);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult EditPerformance()
        {
            return View();
        }

        public IActionResult GenerateUserPerfomance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserInfoModel request)
        {
            var user = await _db.UsersInfo
                .SingleOrDefaultAsync(ui => ui.LoginEmail == request.LoginEmail && ui.Password == request.Password);

            if (user == null)
            {
                throw new InvalidOperationException("The user with this credentials doesn`t exist");
            }
            if (user.Role.ToLower() == "admin")
            {
                return RedirectToAction("AdminTable", user);
            }
            if (user.Role.ToLower() == "user")
            {
                return RedirectToAction("UserTable", user);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInfoModel request, CancellationToken cancellationToken)
        {
            var entity = await _db.UsersInfo
                .SingleOrDefaultAsync(ui => ui.LoginEmail == request.LoginEmail);

            if (entity != null)
            {
                throw new InvalidOperationException("This user already exist");
            }

            var tr = _db.Database.BeginTransaction();
            try
            {
                var createEntity = new UserInfo
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Surname = request.Surname,
                    Password = request.Password,
                    LoginEmail = request.LoginEmail,
                    Role = request.Role,
                };

                _db.Add(createEntity);
                await _db.SaveChangesAsync(cancellationToken);

                tr.Commit();
            }
            catch (Exception)
            {
                tr.Rollback();
                throw new InvalidOperationException("The user was no added");
            }

            ModelState.Clear();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPerformance(UserInfoModel request, CancellationToken cancellationToken)
        {
            var user = await _db.UsersInfo
                .SingleOrDefaultAsync(ui => ui.LoginEmail == request.LoginEmail && ui.Name == request.Name);

            if (user == null)
            {
                throw new InvalidOperationException("The user with this credentials doesn`t exist");
            }

            user.PerformanceScore = request.PerformanceScore;

            await _db.SaveChangesAsync(cancellationToken);

            ModelState.Clear();

            return View(user.Id);
        }

        [HttpPost]
        public async Task<string> GenerateUserPerfomance(UserInfoModel request)
        {
            var user = await _db.UsersInfo
                .SingleOrDefaultAsync(ui => ui.LoginEmail == request.LoginEmail && ui.Name == request.Name);

            if (user == null)
            {
                throw new InvalidOperationException("The user with this credentials doesn`t exist");
            }

            return $"The user: {user.Name} have the performance score: {user.PerformanceScore} (stars - out of 5 stars) ";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}