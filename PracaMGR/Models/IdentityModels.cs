using Microsoft.AspNet.Identity.EntityFramework;

namespace PracaMGR.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<PracaMGR.Posty> Posties { get; set; }

        public System.Data.Entity.DbSet<PracaMGR.Odpowiedzi> Odpowiedzis { get; set; }

        public System.Data.Entity.DbSet<PracaMGR.DniSwiateczne> DniSwiatecznes { get; set; }
    }
}