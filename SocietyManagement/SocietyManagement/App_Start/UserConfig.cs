using SocietyManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace SocietyManagement
{
    public class UserConfig
    {
        public void CreateRoles()
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Comity"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Comity";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }            
        }                
    }
}
