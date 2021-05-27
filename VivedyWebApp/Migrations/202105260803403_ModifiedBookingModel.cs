namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedBookingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rotations", "StartTime_RotationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rotations", "StartTime_RotationId");
            AddForeignKey("dbo.Rotations", "StartTime_RotationId", "dbo.Rotations", "RotationId");
            DropColumn("dbo.Rotations", "StartTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rotations", "StartTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Rotations", "StartTime_RotationId", "dbo.Rotations");
            DropIndex("dbo.Rotations", new[] { "StartTime_RotationId" });
            DropColumn("dbo.Rotations", "StartTime_RotationId");
        }
    }
}
