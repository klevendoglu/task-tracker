namespace TaskTracker.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectManagementEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ProjectManagement.ProjectManagers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ProjectManagement.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AbpUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "ProjectManagement.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        Name = c.String(nullable: false, maxLength: 250),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
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
                    { "DynamicFilter_Project_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "ProjectManagement.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        Name = c.String(nullable: false, maxLength: 250),
                        ProjectId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        EstimatedDays = c.Int(nullable: false),
                        AgentId = c.Long(),
                        AssignTime = c.DateTime(),
                        ClosingTime = c.DateTime(),
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
                    { "DynamicFilter_ProjectTask_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.AgentId)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("ProjectManagement.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.AgentId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "ProjectManagement.ProjectTaskAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskId = c.Int(nullable: false),
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
                    { "DynamicFilter_ProjectTaskAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Documents.Attachments", t => t.AttachmentId, cascadeDelete: true)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("ProjectManagement.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.AttachmentId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "Documents.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        FileName = c.String(),
                        RefId = c.String(),
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
                    { "DynamicFilter_Attachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "ProjectManagement.TaskLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskId = c.Int(nullable: false),
                        Notes = c.String(nullable: false, maxLength: 2500),
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
                    { "DynamicFilter_TaskLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("ProjectManagement.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "ProjectManagement.TaskLogAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskLogId = c.Int(nullable: false),
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
                    { "DynamicFilter_TaskLogAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Documents.Attachments", t => t.AttachmentId, cascadeDelete: true)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("ProjectManagement.TaskLogs", t => t.TaskLogId, cascadeDelete: true)
                .Index(t => t.TaskLogId)
                .Index(t => t.AttachmentId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "ProjectManagement.ToDos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                        IsComplete = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ToDo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ProjectManagement.ToDos", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ToDos", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ToDos", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectManagers", "UserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogAttachments", "TaskLogId", "ProjectManagement.TaskLogs");
            DropForeignKey("ProjectManagement.TaskLogAttachments", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogAttachments", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogAttachments", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogAttachments", "AttachmentId", "Documents.Attachments");
            DropForeignKey("ProjectManagement.TaskLogs", "TaskId", "ProjectManagement.Tasks");
            DropForeignKey("ProjectManagement.TaskLogs", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogs", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.TaskLogs", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectTaskAttachments", "TaskId", "ProjectManagement.Tasks");
            DropForeignKey("ProjectManagement.ProjectTaskAttachments", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectTaskAttachments", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectTaskAttachments", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectTaskAttachments", "AttachmentId", "Documents.Attachments");
            DropForeignKey("Documents.Attachments", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("Documents.Attachments", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("Documents.Attachments", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Tasks", "ProjectId", "ProjectManagement.Projects");
            DropForeignKey("ProjectManagement.Tasks", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Tasks", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Tasks", "CreatorUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Tasks", "AgentId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.ProjectManagers", "ProjectId", "ProjectManagement.Projects");
            DropForeignKey("ProjectManagement.Projects", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Projects", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("ProjectManagement.Projects", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("ProjectManagement.ToDos", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.ToDos", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.ToDos", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.TaskLogAttachments", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.TaskLogAttachments", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.TaskLogAttachments", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.TaskLogAttachments", new[] { "AttachmentId" });
            DropIndex("ProjectManagement.TaskLogAttachments", new[] { "TaskLogId" });
            DropIndex("ProjectManagement.TaskLogs", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.TaskLogs", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.TaskLogs", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.TaskLogs", new[] { "TaskId" });
            DropIndex("Documents.Attachments", new[] { "CreatorUserId" });
            DropIndex("Documents.Attachments", new[] { "LastModifierUserId" });
            DropIndex("Documents.Attachments", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.ProjectTaskAttachments", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.ProjectTaskAttachments", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.ProjectTaskAttachments", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.ProjectTaskAttachments", new[] { "AttachmentId" });
            DropIndex("ProjectManagement.ProjectTaskAttachments", new[] { "TaskId" });
            DropIndex("ProjectManagement.Tasks", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.Tasks", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.Tasks", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.Tasks", new[] { "AgentId" });
            DropIndex("ProjectManagement.Tasks", new[] { "ProjectId" });
            DropIndex("ProjectManagement.Projects", new[] { "CreatorUserId" });
            DropIndex("ProjectManagement.Projects", new[] { "LastModifierUserId" });
            DropIndex("ProjectManagement.Projects", new[] { "DeleterUserId" });
            DropIndex("ProjectManagement.ProjectManagers", new[] { "UserId" });
            DropIndex("ProjectManagement.ProjectManagers", new[] { "ProjectId" });
            DropTable("ProjectManagement.ToDos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ToDo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.TaskLogAttachments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TaskLogAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.TaskLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TaskLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("Documents.Attachments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Attachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.ProjectTaskAttachments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProjectTaskAttachment_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.Tasks",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProjectTask_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.Projects",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Project_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("ProjectManagement.ProjectManagers");
        }
    }
}
