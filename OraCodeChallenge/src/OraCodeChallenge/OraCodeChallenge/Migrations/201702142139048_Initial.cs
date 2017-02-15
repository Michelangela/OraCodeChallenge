namespace OraCodeChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessage",
                c => new
                    {
                        ChatMessageId = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        FromUserId = c.Int(nullable: false),
                        ToUserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.ChatMessageId)
                .ForeignKey("dbo.Chat", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ChatId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        Salt = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessage", "ChatId", "dbo.Chat");
            DropIndex("dbo.ChatMessage", new[] { "ChatId" });
            DropTable("dbo.User");
            DropTable("dbo.Chat");
            DropTable("dbo.ChatMessage");
        }
    }
}
