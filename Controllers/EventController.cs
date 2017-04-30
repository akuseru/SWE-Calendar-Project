using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cal.Data;
using cal.Models;
using cal.Models.EventViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cal.Controllers
{
	public class EventController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public EventController(
		    SignInManager<ApplicationUser> signInManager,
		    UserManager<ApplicationUser> userManager,
		    ApplicationDbContext context
		)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_context = context;
		}
		public async Task<IActionResult> Create(string week, Guid roomId)
		{
			var user = await GetCurrentUserAsync();
			if (user == null)
				return Redirect("/");
			else
			{
				DateTime WeekOf;
				if (String.IsNullOrEmpty(week))
					WeekOf = DateTime.Today;
				else
					WeekOf = DateTime.Parse(week);

				var currentDayOfWeek = (int)WeekOf.DayOfWeek;
				var sunday = WeekOf.AddDays(-currentDayOfWeek);
				var events = (roomId == null) ? new List<RoomReservation>() : await _context.RoomReservation.Where(i => i.Room.ID == roomId && i.Start > sunday && i.Start < sunday.AddDays(7)).ToListAsync();
				return View(new CreateViewModel
				{
					Sunday = sunday,
					RoomId = roomId,
					Rooms = await _context.Rooms.ToListAsync(),
					Events = events,
				});
			}
		}

		public async Task<IActionResult> EventPeople(DateTime day, Guid RoomId, string guests, string addperson = null)
		{
			var user = await GetCurrentUserAsync();
			if (user == null)
				return Redirect("/");
			var r = await _context.Rooms.FirstOrDefaultAsync(i => i.ID == RoomId);
			if (r == null)
				return Redirect("/Event/Create?danger=Room Not Found");


		    List<string> guestsList;
		    if (!string.IsNullOrEmpty(guests))
		        guestsList = new List<string>(guests.Replace(" ", "").Split(','));
		    else
		        guestsList = new List<string>();
		    
			if (guestsList.Contains(user.Id))
			    guestsList.Remove(user.Id);
			var gl = new List<ApplicationUser>
	    {
		user
	    };

		    if (!string.IsNullOrEmpty(addperson))
		    {
		        addperson = addperson.ToUpperInvariant();
                var addguest = await _userManager.Users.FirstOrDefaultAsync(i => i.NormalizedEmail == addperson);
		        if (addguest == null)
		        {
		            ViewData["danger"] = "User not found";
		        } else
		        {
                    gl.Add(addguest);
		            ViewData["msg"] = "user added";
		        }
		    }

            var events = new List<CalendarEvent>();
			foreach (var s in guestsList)
			{
				var toview = await _userManager.Users.FirstAsync(i => i.Id == s);
				gl.Add(toview);
				if (await _context.CanView(user, toview))
				{
					events.AddRange(await _context.LoadUserEvents(toview, user, day, true));
				}
			}
			return View(new CreateViewModel
			{
				Sunday = day,
				Room = r,
				Guests = gl,
				UserEvents = events,
			});
		}

		public async Task<IActionResult> CreateFinal(DateTime day, Guid RoomId, string guests, string start, string end)
		{
			var user = await GetCurrentUserAsync();
			if (user == null)
				return Redirect("/");

			var r = await _context.Rooms.FirstOrDefaultAsync(i => i.ID.Equals(RoomId) );
			if (r == null)
				return Redirect("/Event/Create?danger=Room Not Found");

		    List<string> guestsList;
		    if (!string.IsNullOrEmpty(guests))
		        guestsList = new List<string>(guests.Replace(" ", "").Split(','));
		    else
		        guestsList = new List<string>();


            //guests + me = 3
            if (guestsList.Count() < 2)
		        return Redirect("/Event/Create?danger=Not enough Participants");

            //guests + me = 10
		    if (guestsList.Count() > 9)
		        return Redirect("/Event/Create?danger=Too Many Participants");

            var eventStart = day;
			var eventEnd = day;
			var temp = start.Split(':').Select(i => int.Parse(i)).ToList();
			eventStart = eventStart.AddHours(temp[0]);
			eventStart = eventStart.AddMinutes(temp[1]);

			temp = end.Split(':').Select(i => int.Parse(i)).ToList();
			eventEnd = eventEnd.AddHours(temp[0]);
			eventEnd = eventEnd.AddHours(temp[1]);

			var gl = new List<ApplicationUser>();
			foreach (var s in guestsList)
			{
				var toview = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == s);
				gl.Add(toview);
			}

			var success = await _context.CreateEvent(r, user, gl, eventStart, eventEnd);

			if (!success.Item1)
				return Redirect("/Event/EventPeople?RoomId=" + r.ID + "&day=" + string.Format("{0:yyyy-M-d dddd}", day) + "&guests="+ string.Join(",", gl.Select(i => i.Id)) + "&danger="+success.Item2);

			return Redirect("/?msg=Event Created!");
		}

		private Task<ApplicationUser> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}
	}
}