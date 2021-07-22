namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRoomsAndRoomsFkAndUserRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        SeatsLayout = c.String(nullable: false),
                        CinemaId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RoomId)
                .ForeignKey("dbo.Cinemas", t => t.CinemaId, cascadeDelete: true)
                .Index(t => t.CinemaId);
            
            AddColumn("dbo.Screenings", "RoomId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Movies", "UserRating", c => c.Int(nullable: false));
            CreateIndex("dbo.Screenings", "RoomId");
            AddForeignKey("dbo.Screenings", "RoomId", "dbo.Rooms", "RoomId", cascadeDelete: true);
            DropColumn("dbo.Cinemas", "SeatsLayout");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cinemas", "SeatsLayout", c => c.String(nullable: false));
            DropForeignKey("dbo.Screenings", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "CinemaId", "dbo.Cinemas");
            DropIndex("dbo.Rooms", new[] { "CinemaId" });
            DropIndex("dbo.Screenings", new[] { "RoomId" });
            DropColumn("dbo.Movies", "UserRating");
            DropColumn("dbo.Screenings", "RoomId");
            DropTable("dbo.Rooms");
        }
    }
}
