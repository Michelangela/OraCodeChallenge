namespace OraCodeChallenge.Migrations
{
    using Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OraCodeChallenge.Models.OraContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "OraCodeChallenge.Models.OraContext";
        }

        protected override void Seed(OraCodeChallenge.Models.OraContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            //context.Roles.AddOrUpdate(r => r.Name,
            //        new Role { Name = "User" },
            //        new Role { Name = "Admin" },
            //        new Role { Name = "Sales" });

            context.Chats.AddOrUpdate(new Chat
            {
                ChatId = 1
            });
        }
    }
}
