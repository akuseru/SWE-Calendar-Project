using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cal.Models
{
    public enum ReservationType : int
    { 
        Admin,
        Holiday,
        Regular,
        Proposal,
    }
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class RoomReservation
    {
        public Guid ID {get;set;}
        public Room Room {get;set;}
        public ApplicationUser Owner {get;set;}
        public DateTime Start {get;set;}
        public DateTime End {get;set;}
        public IEnumerable<ApplicationUser> Participants {get;set;}
        public ReservationType ReservationType {get;set;}
    }

    public class CalendarEvent
    {
        public Guid Id {get;set;}
        public RoomReservation Event {get;set;}
        public ApplicationUser User {get;set;}
        public DateTime Start {get;set;}
        public DateTime End {get;set;}
        public ReservationType ReservationType {get;set;} 
    }
    public enum ViewType : int 
    {
        Manager,
        Regular
    }
    public class Viewers
    {
        public virtual ApplicationUser Owner {get;set;} 
        public virtual ApplicationUser Viewer {get;set;}

        [Key]
        public Guid Id{get;set;}
        
        // [ForeignKey("ApplicationUser"), Column(Order = 0)]
        public string OwnerId {get;set;}
        
        // [ForeignKey("ApplicationUser"), Column(Order = 0)]
        public string ViewerId {get;set;}
        public ViewType ViewType {get;set;} 

    }
}
