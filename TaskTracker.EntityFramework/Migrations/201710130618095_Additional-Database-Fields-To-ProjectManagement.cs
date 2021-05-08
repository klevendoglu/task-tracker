namespace TaskTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalDatabaseFieldsToProjectManagement : DbMigration
    {
        public override void Up()
        {
            AddColumn("ProjectManagement.Projects", "Status", c => c.Int(nullable: false));
            AddColumn("ProjectManagement.Projects", "ClosingTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("ProjectManagement.Projects", "ClosingTime");
            DropColumn("ProjectManagement.Projects", "Status");
        }
    }
}
