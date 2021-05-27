namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRotationField : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            DropIndex("dbo.Rotations", new[] { "StartTime_RotationId" });
            AlterColumn("dbo.Rotations", "MovieId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Rotations", "StartTime_RotationId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "TrailerUrl", c => c.String(nullable: false));
            CreateIndex("dbo.Rotations", "MovieId");
            CreateIndex("dbo.Rotations", "StartTime_RotationId");
            AddForeignKey("dbo.Rotations", "MovieId", "dbo.Movies", "MovieId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropIndex("dbo.Rotations", new[] { "StartTime_RotationId" });
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            AlterColumn("dbo.Movies", "TrailerUrl", c => c.String());
            AlterColumn("dbo.Movies", "Description", c => c.String());
            AlterColumn("dbo.Movies", "Category", c => c.String());
            AlterColumn("dbo.Movies", "Name", c => c.String());
            AlterColumn("dbo.Rotations", "StartTime_RotationId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Rotations", "MovieId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rotations", "StartTime_RotationId");
            CreateIndex("dbo.Rotations", "MovieId");
            AddForeignKey("dbo.Rotations", "MovieId", "dbo.Movies", "MovieId");
        }
    }
}
