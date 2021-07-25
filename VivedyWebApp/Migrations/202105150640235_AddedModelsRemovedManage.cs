namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelsRemovedManage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        RotationId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Rotations", t => t.RotationId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.RotationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Rating = c.Int(nullable: false),
                        Category = c.String(),
                        Duration = c.Time(nullable: false, precision: 7),
                        Price = c.Int(nullable: false),
                        TrailerUrl = c.String(),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Rotations",
                c => new
                    {
                        RotationId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        MovieId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RotationId)
                .ForeignKey("dbo.Movies", t => t.MovieId)
                .Index(t => t.MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations");
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            DropIndex("dbo.Bookings", new[] { "UserId" });
            DropIndex("dbo.Bookings", new[] { "RotationId" });
            DropTable("dbo.Rotations");
            DropTable("dbo.Movies");
            DropTable("dbo.Bookings");
        }
    }
}
