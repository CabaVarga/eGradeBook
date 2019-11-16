namespace eGradeBook.Migrations
{
    using eGradeBook.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<eGradeBook.Infrastructure.GradeBookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(eGradeBook.Infrastructure.GradeBookContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //UserManager<GradeBookUser, int> _userManager =
            //    new UserManager<GradeBookUser, int>(
            //    new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

            //RoleManager<CustomRole, int> _roleManager =
            //    new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));

            //#region Roles
            //CustomRole adminRole = new CustomRole() { Name = "admins" };
            //CustomRole studentRole = new CustomRole() { Name = "students" };
            //CustomRole teacherRole = new CustomRole() { Name = "teachers" };
            //CustomRole parentRole = new CustomRole() { Name = "parents" };

            //_roleManager.Create(adminRole);
            //_roleManager.Create(studentRole);
            //_roleManager.Create(teacherRole);
            //_roleManager.Create(parentRole);
            //#endregion

        }
    }
}
