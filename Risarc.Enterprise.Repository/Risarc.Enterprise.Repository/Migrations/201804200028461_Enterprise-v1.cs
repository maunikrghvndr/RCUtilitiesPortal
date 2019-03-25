namespace Risarc.Enterprise.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Enterprisev1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        AppID = c.Int(nullable: false, identity: true),
                        AppName = c.String(),
                        AppPath = c.String(),
                        IsConsoleApp = c.Boolean(nullable: false),
                        AssemblyPath = c.String(),
                        ClassName = c.String(),
                        EnteredAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AppID);
            
            CreateTable(
                "dbo.DependantJobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentJobID = c.Int(nullable: false),
                        DependantJobID = c.Int(nullable: false),
                        RunTimeParams = c.String(),
                        ParentRan = c.Boolean(nullable: false),
                        DependancyGroup = c.Int(),
                        DependancyOrder = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FileImportingJobs",
                c => new
                    {
                        FileImportingJobID = c.Int(nullable: false, identity: true),
                        IdentifyingPath = c.String(),
                        IdentifyingFileName = c.String(),
                        JobID = c.Int(nullable: false),
                        ImportsFromDirectory = c.String(),
                    })
                .PrimaryKey(t => t.FileImportingJobID);
            
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        JobCategoryID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.JobCategoryID);
            
            CreateTable(
                "dbo.JobLogs",
                c => new
                    {
                        JobLogID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        LogDetail = c.String(),
                        LogDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobLogID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                        JobDescription = c.String(),
                        JobParameters = c.String(),
                        AppID = c.Int(nullable: false),
                        EnteredAt = c.DateTime(nullable: false),
                        JobProcessID = c.Int(),
                        LastStatusDetail = c.String(),
                        LastStatusTime = c.DateTime(),
                        JobStatusTypeID = c.Int(),
                        Active = c.Boolean(nullable: false),
                        JobAsyncTaskID = c.Int(),
                        ActivateTransaction = c.Boolean(nullable: false),
                        JobCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobID)
                .ForeignKey("dbo.Applications", t => t.AppID, cascadeDelete: true)
                .ForeignKey("dbo.JobStatusTypes", t => t.JobStatusTypeID)
                .Index(t => t.AppID)
                .Index(t => t.JobStatusTypeID);
            
            CreateTable(
                "dbo.JobServers",
                c => new
                    {
                        JobServerID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        ServerID = c.Int(nullable: false),
                        MaxInstances = c.Int(nullable: false),
                        CurrentInstances = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobServerID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.JobStatusTypes",
                c => new
                    {
                        JobStatusTypeID = c.Int(nullable: false, identity: true),
                        JobStatus = c.String(),
                        JobStatusDesc = c.String(),
                    })
                .PrimaryKey(t => t.JobStatusTypeID);
            
            CreateTable(
                "dbo.JobSchedules",
                c => new
                    {
                        JobScheduleID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        IntervalInMilliseconds = c.Int(nullable: false),
                        RunDays = c.String(),
                        StartDateTime = c.DateTime(),
                        NextStartDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobScheduleID);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        ServerID = c.Int(nullable: false, identity: true),
                        ServerName = c.String(),
                        ServerIP = c.String(),
                    })
                .PrimaryKey(t => t.ServerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "JobStatusTypeID", "dbo.JobStatusTypes");
            DropForeignKey("dbo.JobServers", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.JobLogs", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "AppID", "dbo.Applications");
            DropIndex("dbo.JobServers", new[] { "JobID" });
            DropIndex("dbo.Jobs", new[] { "JobStatusTypeID" });
            DropIndex("dbo.Jobs", new[] { "AppID" });
            DropIndex("dbo.JobLogs", new[] { "JobID" });
            DropTable("dbo.Servers");
            DropTable("dbo.JobSchedules");
            DropTable("dbo.JobStatusTypes");
            DropTable("dbo.JobServers");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobLogs");
            DropTable("dbo.JobCategories");
            DropTable("dbo.FileImportingJobs");
            DropTable("dbo.DependantJobs");
            DropTable("dbo.Applications");
        }
    }
}
