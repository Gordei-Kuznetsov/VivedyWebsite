namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedRotationsToScreenings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rotations", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations");
            DropIndex("dbo.Bookings", new[] { "RotationId" });
            DropIndex("dbo.Rotations", new[] { "MovieId" });
            CreateTable(
                "dbo.Screenings",
                c => new
                    {
                        ScreeningId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        MovieId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ScreeningId)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId);
            
            AddColumn("dbo.Bookings", "ScreeningId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Bookings", "ScreeningId");
            AddForeignKey("dbo.Bookings", "ScreeningId", "dbo.Screenings", "ScreeningId", cascadeDelete: true);
            DropColumn("dbo.Bookings", "RotationId");
            DropTable("dbo.Rotations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rotations",
                c => new
                    {
                        RotationId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        MovieId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RotationId);
            
            AddColumn("dbo.Bookings", "RotationId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Bookings", "ScreeningId", "dbo.Screenings");
            DropForeignKey("dbo.Screenings", "MovieId", "dbo.Movies");
            DropIndex("dbo.Screenings", new[] { "MovieId" });
            DropIndex("dbo.Bookings", new[] { "ScreeningId" });
            DropColumn("dbo.Bookings", "ScreeningId");
            DropTable("dbo.Screenings");
            CreateIndex("dbo.Rotations", "MovieId");
            CreateIndex("dbo.Bookings", "RotationId");
            AddForeignKey("dbo.Bookings", "RotationId", "dbo.Rotations", "RotationId", cascadeDelete: true);
            AddForeignKey("dbo.Rotations", "MovieId", "dbo.Movies", "MovieId", cascadeDelete: true);
        }
    }
}
