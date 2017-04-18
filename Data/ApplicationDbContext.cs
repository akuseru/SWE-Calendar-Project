using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using cal.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cal.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{

		public DbSet<Room> Rooms { get; set; }
		public DbSet<RoomReservation> RoomReservation { get; set; }
		public DbSet<CalendarEvent> Events { get; set; }
		public DbSet<Viewers> Viewers { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		    : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}



		/// <summary>
		/// Loads a users events for a given week.
		/// </summary>
		/// <param name="ForUser">The user who we are getting events for</param>
		/// <param name="Me">The current user who is requesting the data</param>
		/// <param name="week">any day for the current week.</param>
		/// <param name="SingleDay">if true only load up events for a single day</param>
		/// <returns>a list of events. If teh user is not allowed to view the forusers events we return an empty set.</returns>
		public async Task<List<CalendarEvent>> LoadUserEvents(ApplicationUser ForUser, ApplicationUser Me, DateTime week, bool SingleDay = false)
		{
			//reset to sunday to get the events for the week.
			if (week.DayOfWeek != DayOfWeek.Sunday)
			{
				week = week.AddDays(-(int)week.DayOfWeek);
			}

			//
			if (!await CanView(Me, ForUser))
			{
				return new List<CalendarEvent>();
			}
			DateTime End = week.AddDays(7);
			if (SingleDay)
			{
				End = week.AddDays(1);
			}

			return await Events.Where(i => i.User == ForUser && i.Start > week && i.End < End).ToListAsync();
		}
		/// <summary>
		/// Checks to see if a specific user can view someone elses calendar.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="me">Me.</param>
		/// <param name="UserMeWantsToView">User me wants to view.</param>
		public async Task<bool> CanView(ApplicationUser me, ApplicationUser UserMeWantsToView)
		{
			if (me == UserMeWantsToView)
				return true;
			if (me.Role == UserRole.AdminAssistant || me.Role == UserRole.Administrator)
				return true;
			var found = await Viewers.FirstOrDefaultAsync(i => i.Owner == UserMeWantsToView && i.Viewer == me);

			return found != default(Viewers);
		}

		/// <summary>
		/// create an event.
		/// </summary>
		/// <returns>The event.</returns>
		/// <param name="room">Room.</param>
		/// <param name="user">Creator.</param>
		/// <param name="participants">Participants.</param>
		/// <param name="eventStart">Start.</param>
		/// <param name="eventEnd">End.</param>
		public async Task<Tuple<bool, string>> CreateEvent(Room room, ApplicationUser user, List<ApplicationUser> participants, DateTime eventStart, DateTime eventEnd)
		{
			if (eventStart <= eventEnd)
				return new Tuple<bool, string>(false, "End time must be after Start time");
			var newEvent = new RoomReservation
			{
				ID = new Guid(),
				Room = room,
				Owner = user,
				Start = eventStart,
				End = eventEnd,
				Participants = participants,
				ReservationType = ReservationType.Regular
			};
			RoomReservation.Add(newEvent);

			var calEvent = new CalendarEvent
			{
				Id = new Guid(),
				Event = newEvent,
				User = user,
				Start = eventStart,
				End = eventEnd,
				ReservationType = ReservationType.Regular
			};
			Events.Add(calEvent);
			foreach (var g in participants)
			{
				if (g.Id == user.Id) continue;
				calEvent = new CalendarEvent
				{
					Id = new Guid(),
					Event = newEvent,
					User = g,
					Start = eventStart,
					End = eventEnd,
					ReservationType = ReservationType.Regular
				};
				Events.Add(calEvent);
			}
			await SaveChangesAsync();

			return new Tuple<bool, string>(true, "");

		}

	}
}
