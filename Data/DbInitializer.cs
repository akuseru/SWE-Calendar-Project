using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace cal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            
            #region Populate Test Users
            var Users = new List<ApplicationUser>
                {
                    //Admin
                    new ApplicationUser
                    {
                        FirstName = "Test",
                        LastName = "Admin",
                        Role = UserRole.Administrator,
                        UserName = "adm.drakesystem@gmail.com",
                        Email = "adm.drakesystem@gmail.com",
                        NormalizedEmail = "ADM.DRAKESYSTEM@GMAIL.COM",
                        NormalizedUserName = "ADM.DRAKESYSTEM@GMAIL.COM",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },

                    //AdminAssistant
                    new ApplicationUser
                    {
                        FirstName = "Test",
                        LastName = "AdminAssistant",
                        Role = UserRole.AdminAssistant,
                        UserName = "admassistant.drakesystem@gmail.com",
                        Email = "admassistant.drakesystem@gmail.com",
                        NormalizedEmail = "ADMASSISTANT.DRAKESYSTEM@GMAIL.COM",
                        NormalizedUserName = "ADMASSISTANT.DRAKESYSTEM@GMAIL.COM",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },

                    //Manager
                    new ApplicationUser
                    {
                        FirstName = "Test",
                        LastName = "Manager",
                        Role = UserRole.Manager,
                        UserName = "manager.drakesystem@gmail.com",
                        Email = "manager.drakesystem@gmail.com",
                        NormalizedEmail = "MANAGER.DRAKESYSTEM@GMAIL.COM",
                        NormalizedUserName = "MANAGER.DRAKESYSTEM@GMAIL.COM",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },

                    //User
                    new ApplicationUser
                    {
                        FirstName = "Test",
                        LastName = "User",
                        Role = UserRole.User,
                        UserName = "user.drakesystem@gmail.com",
                        Email = "user.drakesystem@gmail.com",
                        NormalizedEmail = "USER.DRAKESYSTEM@GMAIL.COM",
                        NormalizedUserName = "USER.DRAKESYSTEM@GMAIL.COM",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    }
                };
               
            foreach (ApplicationUser user in Users)
            {
                if (!context.Users.Any(i => i.UserName == user.UserName))
                {

                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "drakesystem");
                    user.PasswordHash = hashed;

                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(user).GetAwaiter().GetResult();
                }
            }

            #endregion

            #region Populate Default Rooms
            List<string> nameOfDefaultRoomList = new List<string> { "C1", "C2", "C3" };

            foreach (string nameOfDefaultRoom in nameOfDefaultRoomList)
            {
                if (!context.Rooms.Any(i => i.Name == nameOfDefaultRoom))
                {
                    var room = new Room
                    {
                        ID = Guid.NewGuid(),
                        Name = nameOfDefaultRoom
                    };

                    context.Rooms.Add(room);
                    context.SaveChanges();
                }
            }
            #endregion
            
        }
    }
}
