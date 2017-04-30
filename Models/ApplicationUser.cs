using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cal.Models
{
    public enum UserRole : int 
    {
        User = 0,
        Administrator = 1,
        AdminAssistant = 2,
        Manager = 3
    }
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get;set;}
        public string LastName{get;set;}
        public UserRole Role{get;set;}
    }
}
