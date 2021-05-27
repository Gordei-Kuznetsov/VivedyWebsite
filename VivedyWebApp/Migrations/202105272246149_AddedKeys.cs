namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedKeys : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "RotationId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Rotations", "MovieId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "RotationId");
            CreateIndex("dbo.Rotations", "MovieId");
            AddForeignKey("dbo.Rotations", "MovieId", "dbo.Movies", "MovieId");
            AddForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations", "RotationId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations");
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            DropIndex("dbo.Bookings", new[] { "RotationId" });
            AlterColumn("dbo.Rotations", "MovieId", c => c.String());
            AlterColumn("dbo.Bookings", "RotationId", c => c.String(nullable: false));
        }
    }
}
