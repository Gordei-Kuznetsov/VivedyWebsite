using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Application's database context class
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Field for Movies in the ApplicationDbContext
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// Field for Bookings in the ApplicationDbContext
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Field for Screenings in the ApplicationDbContext
        /// </summary>
        public DbSet<Screening> Screenings { get; set; }

        /// <summary>
        /// Field for Cinemas in the ApplicationDbContext
        /// </summary>
        public DbSet<Cinema> Cinemas { get; set; }

        /// <summary>
        /// Field for Rooms in the ApplicationDbContext
        /// </summary>
        public DbSet<Room> Rooms { get; set; }
    }
}