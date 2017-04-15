using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cal.Data;
using cal.Models;
using cal.Models.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace cal.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index(string week)
        {
            var user = await GetCurrentUserAsync();
            if(user == null)
                return View("IndexLogin");
            else
            {
                DateTime WeekOf;
                if(String.IsNullOrEmpty(week))
                    WeekOf = DateTime.Today;
                else
                    WeekOf = DateTime.Parse(week);
                
                var currentDayOfWeek = (int) WeekOf.DayOfWeek;
                var sunday = WeekOf.AddDays(-currentDayOfWeek);

                return View(new IndexViewModel{
                    Sunday = sunday,
                    Events = await _context.LoadUserEvents(user, user, sunday),
                });
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
