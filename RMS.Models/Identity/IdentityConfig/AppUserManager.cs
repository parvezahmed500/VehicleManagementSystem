﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using RMS.Models.DatabaseContext;
using RMS.Models.EntityModels;
using RMS.Models.EntityModels.Identity;

namespace RMS.Models.Identity.IdentityConfig
{
    public class AppUserManager:UserManager<AppUser,int>
    {
        public AppUserManager(IUserStore<AppUser, int> store) : base(store)
        {
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> option, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>(context.Get<RmsDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AppUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<AppUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<AppUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            //var dataProtectionProvider = option.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<AppUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        
        }

        public int AddUserForEmployee(Employee employee)
        {
            var user=new AppUser()
            {
                UserName = employee.Email,
                Email = employee.Email
            };
            //string defaultPassword = "Aa@123456";
            var result=this.Create(user, employee.Password);
            if (result.Succeeded)
            {
                var roleAdded=this.AddToRole(user.Id, "User");
                int Id= user.Id;
                return Id;
            }
            return 0;
        }


        public int AddControllerForEmployee(Employee employee)
        {
            var user = new AppUser()
            {
                UserName = employee.Email,
                Email = employee.Email
            };
            //string defaultPassword = "Aa@123456";
            var result = this.Create(user, employee.Password);
            if (result.Succeeded)
            {
                var roleAdded = this.AddToRole(user.Id, "Controller");
                int Id = user.Id;
                return Id;
            }
            return 0;
        }
    }
}
