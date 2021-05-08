namespace TaskTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserWidgetAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Core.UserWidgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        WidgetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Core.Widgets", t => t.WidgetId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.WidgetId);
            
            CreateTable(
                "Core.Widgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Core.UserWidgets", "WidgetId", "Core.Widgets");
            DropForeignKey("Core.Widgets", "RoleId", "dbo.AbpRoles");
            DropForeignKey("Core.UserWidgets", "UserId", "dbo.AbpUsers");
            DropIndex("Core.Widgets", new[] { "RoleId" });
            DropIndex("Core.UserWidgets", new[] { "WidgetId" });
            DropIndex("Core.UserWidgets", new[] { "UserId" });
            DropTable("Core.Widgets");
            DropTable("Core.UserWidgets");
        }
    }
}
