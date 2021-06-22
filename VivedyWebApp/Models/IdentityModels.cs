﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VivedyWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// Application User Identity model
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        //Added new field for the user name because the current 'Username' field is used by the system as an email
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [DisplayName("User Name")]
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

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
        /// Field for Rotations in the ApplicationDbContext
        /// </summary>
        public DbSet<Rotation> Rotations { get; set; }

        /// <summary>
        /// Field for Cinemas in the ApplicationDbContext
        /// </summary>
        public DbSet<Cinema> Cinemas { get; set; }

    }
}