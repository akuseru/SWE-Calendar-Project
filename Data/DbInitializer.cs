using System;
using System.Linq;
using cal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            
            // Look for any students.
            var Users = 
                new ApplicationUser{
                    FirstName="Adam", 
                    LastName="Smith", 
                    Role=UserRole.Administrator, 
                    UserName="adam@akuseru.io", 
                    Email="adam@akuseru.io",
                    NormalizedEmail="ADAM@AKUSERU.IO",
                    NormalizedUserName="ADAM@AKUSERU.IO",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(i => i.UserName == Users.UserName))
            {
                
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(Users,"secret");
                Users.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(Users);
            }
            

        }
    }
}