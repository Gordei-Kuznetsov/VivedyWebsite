namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedKeyAttributes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations");
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "RotationId" });
            DropIndex("dbo.Bookings", new[] { "UserId" });
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            AlterColumn("dbo.Bookings", "RotationId", c => c.String());
            AlterColumn("dbo.Bookings", "UserId", c => c.String());
            AlterColumn("dbo.Rotations", "MovieId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rotations", "MovieId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Bookings", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Bookings", "RotationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rotations", "MovieId");
            CreateIndex("dbo.Bookings", "UserId");
            CreateIndex("dbo.Bookings", "RotationId");
            AddForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations", "RotationId");
            AddForeignKey("dbo.Rotations", "MovieId", "dbo.Movies", "MovieId");
        }
    }
}
