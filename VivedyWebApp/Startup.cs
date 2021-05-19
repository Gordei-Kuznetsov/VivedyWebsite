using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VivedyWebApp.Models;

[assembly: OwinStartupAttribute(typeof(VivedyWebApp.Startup))]
namespace VivedyWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //CreateRoles();
        }
        /*
        private void CreateRoles()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            // Used to create 2 Roles (Admin, Visitor)
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Visitor"))
            {
                var role = new IdentityRole();
                role.Name = "Visitor";
                roleManager.Create(role);
            }
        }*/
    }
}
