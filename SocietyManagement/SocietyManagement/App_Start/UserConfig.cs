using SocietyManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Data.Entity;

namespace SocietyManagement
{
    public static class UserConfig
    {
        public static void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Super"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Super";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "SuperUser";
                user.FirstName = "Super";
                user.LastName = "User";
                user.Email = "mukeshbpatel@gmail.com";
                user.PhoneNumber = "9860002040";
                user.DOB = new System.DateTime(1983, 2, 20);
                user.Gender = "Male";

                string userPWD = "Anjana$45@7";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Super");

                }
            }
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
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                using (SocietyManagementEntities db = new SocietyManagementEntities())
                {
                    var lst = db.BuildingUnits.ToList().Where(u => u.OwnerID == null);
                    if (lst.Any())
                        foreach (var item in lst)
                        {
                            var user = UserManager.Users.Where(u => u.UserName == item.UDK1).FirstOrDefault();
                            if (user == null)
                            {
                                user = new ApplicationUser();
                                user.UserName = item.UDK1;
                                user.LastName = item.OwnerName.Split(' ')[0];
                                user.FirstName = item.OwnerName.Replace(user.LastName, "").Trim();
                                if (!string.IsNullOrEmpty(item.UDK2))
                                    user.PhoneNumber = item.UDK2;
                                user.Gender = "Male";
                                string userPWD = "abcd@1234";
                                var chkUser = UserManager.Create(user, userPWD);

                                //Add default User to Role User    
                                if (chkUser.Succeeded)
                                {
                                    var result1 = UserManager.AddToRole(user.Id, "User");

                                    var unit = db.BuildingUnits.Find(item.UnitID);
                                    if (unit != null)
                                    {
                                        unit.OwnerID = user.Id;
                                    }
                                    db.Entry(unit).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    string a = "please stop";
                                }
                            } else
                            {
                                var unit = db.BuildingUnits.Find(item.UnitID);
                                if (unit != null)
                                {
                                    unit.OwnerID = user.Id;
                                }
                                db.Entry(unit).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                }
            }
        }
    }
}
