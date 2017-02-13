namespace OraCodeChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropTable("dbo.Role");
            DropTable("dbo.UserRole");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserRole", "UserId");
            CreateIndex("dbo.UserRole", "RoleId");
            AddForeignKey("dbo.UserRole", "RoleId", "dbo.Role", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRole", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
    }
}
