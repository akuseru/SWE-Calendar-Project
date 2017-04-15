using System;
using System.Collections.Generic;

namespace cal.Models.EventViewModel
{
    public class CreateViewModel
    {
        public DateTime Sunday { get; set; }
        public Guid RoomId{get;set;}
        public List<Room> Rooms{get;set;}
        public Room Room {get;set;}

        public List<RoomReservation> Events{get;set;}
        public List<CalendarEvent> UserEvents{get;set;}
        public List<ApplicationUser> Guests{get;set;}
    }
}
