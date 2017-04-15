using System;
using System.Threading.Tasks;
using cal.Data;
using cal.Models;
using cal.Models.AdminViewModels;
using cal.Models.EventViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cal.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> NewRoom()
        {
            var user = await GetCurrentUserAsync();
            if(user == null || user.Role != UserRole.Administrator)
                return Redirect("/");
            
            return View(new NewRoomViewModel{
                Rooms = await _context.Rooms.ToListAsync(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> MakeNewRoom(string name)
        {
            var user = await GetCurrentUserAsync();
            if(user == null || user.Role != UserRole.Administrator)
                return Redirect("/");
            var room = new Room{
                ID = new Guid(),
                Name = name
            };
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return Redirect("/Admin/NewRoom");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            
            var user = await GetCurrentUserAsync();
            if(user == null || user.Role != UserRole.Administrator)
                return Redirect("/");

            var r = await _context.Rooms.FirstOrDefaultAsync(i => i.ID == id);
            _context.Rooms.Remove(r);
            
            
            _context.SaveChanges();

            return Redirect("/Admin/NewRoom");
        }
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
