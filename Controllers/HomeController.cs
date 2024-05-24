using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data;
using StaffManagement.Models;
using System.Diagnostics;

namespace StaffManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult LoginUser(string username, string password)
        {
            var Session = HttpContext.Session;
            try
            {
                if (username != null && password != null)
                {
                    var account = _context.Accounts.Where(u => u.Username == username && u.Password == password && u.IsActive == true).Include(u => u.User).ToList();
                    if (account.Count == 1)
                    {
                        Session.SetInt32("AccountID", account[0].Id);
                        Session.SetString("Name", account[0].User.Name);
                        Session.SetInt32("UserID", account[0].User.Id);
                        if (account[0].IsAdmin)
                        {
                            Session.SetString("Role", "Admin");
                        }
                        else
                        {
                            Session.SetString("Role", "Staff");
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session.SetInt32("AccountID", -1);
                        ViewBag.message = "User Name or Password is incorrect!";
                    }
                }
                else
                {
                    Session.SetInt32("AccountID", -1);
                    ViewBag.message = "Some unexpected issue is occure please try again!";
                }
            }
            catch (Exception ex)
            {
                Session.SetInt32("AccountID", -1);
                ViewBag.message = "Some unexpected issue is occure please try again!";
            }
            return View("Login");
        }

        public IActionResult LogOut()
        {
            var Session = HttpContext.Session;
            Session.SetInt32("AccountID", -1);
            return RedirectToAction("Login");
        }
    }
}
