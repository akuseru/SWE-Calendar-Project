using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cal.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Room
    {
        public Guid ID {get;set;}
        public string Name{get;set;}
        
    }
}
