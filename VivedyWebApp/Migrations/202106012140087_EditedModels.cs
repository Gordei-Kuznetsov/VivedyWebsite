namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rotations", "StartTime_RotationId", "dbo.Rotations");
            DropIndex("dbo.Rotations", new[] { "StartTime_RotationId" });
            AddColumn("dbo.Rotations", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Rotations", "StartTime_RotationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rotations", "StartTime_RotationId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Rotations", "StartTime");
            CreateIndex("dbo.Rotations", "StartTime_RotationId");
            AddForeignKey("dbo.Rotations", "StartTime_RotationId", "dbo.Rotations", "RotationId");
        }
    }
}
