using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracaMGR.Models;

namespace PracaMGR.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PracaMGR.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PracaMGR.Models.ApplicationDbContext context)
        {
           var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

           context.Roles.AddOrUpdate(p => p.Name,
             new IdentityRole() { Id = "1", Name = "Admin" },
             new IdentityRole() { Id = "2", Name = "SimpleUser" });

            var user = new ApplicationUser() { UserName = "Admin" } ;

            if (userManager.FindByName("Admin") == null)
            {
                var result = userManager.Create(user, "Admin.2015");

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }    

            }           
        }
    }
}
