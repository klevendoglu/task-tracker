namespace TaskTracker.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectAttachment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ProjectManagement.ProjectAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        AttachmentId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProjectAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Documents.Attachments", t => t.AttachmentId, cascadeDelete: true)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("ProjectManagement.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.AttachmentId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ProjectManagement.ProjectAttachments", "ProjectId", "ProjectManagement.Projects");
            DropForeignKey("ProjectManagement.ProjectAttachments", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectAttachments", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectAttachments", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectAttachments", "AttachmentId", "Documents.Attachments");
            DropIndex("ProjectManagement.ProjectAttachments", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.ProjectAttachments", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.ProjectAttachments", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.ProjectAttachments", new[] { "AttachmentId" });
            DropIndex("ProjectManagement.ProjectAttachments", new[] { "ProjectId" });
            DropTable("ProjectManagement.ProjectAttachments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProjectAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
