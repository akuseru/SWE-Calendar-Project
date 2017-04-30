
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
    public class CalendarController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CalendarController(
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
            return Redirect("/");
        }

        public async Task<IActionResult> Shared()
        {
			var user = await GetCurrentUserAsync();
			if (user == null)
				return Redirect("/");
            
            ViewData["Message"] = "Your application description page.";
            ViewData["SharedWithMe"] = await _context.SharedWithMe(user);
            ViewData["IShareWith"] = await _context.IShareWith(user);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Grant(string email)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Redirect("/");
            var _target = _context.Users.FirstOrDefault(i => i.NormalizedEmail == email.ToUpper());
            if (_target == null)
                return Redirect("/Calendar/Shared?msg=user not found");
            if(! await _context.CanView(_target, user))
            {
                _context.Viewers.Add(new Viewers{
                    Owner = user,
                    Viewer = _target,
                    ViewType = ViewType.Regular
                });
                _context.SaveChanges();
                return Redirect("/Calendar/Shared?msg=Added viewer");
            }
            return Redirect("/Calendar/Shared?msg=User can already view cal");
        }
        [HttpPost]
        public async Task<IActionResult> Revoke(string id)
        {
			var user = await GetCurrentUserAsync();
			if (user == null)
				return Redirect("/");

            var _target = _context.Users.FirstOrDefault(i => i.Id == id);
            if(_target == null)
                return Redirect("/Calendar/Shared?msg=user not found");

            if(await _context.CanView(_target, user))
            {
                var viewrow = _context.Viewers.FirstOrDefault(i => i.Owner == user && i.Viewer == _target);
                if(viewrow != null)
                {
                    _context.Viewers.Remove(viewrow);
                    _context.SaveChanges();
                }
            }
            return Redirect("/Calendar/Shared?msg=ok");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
