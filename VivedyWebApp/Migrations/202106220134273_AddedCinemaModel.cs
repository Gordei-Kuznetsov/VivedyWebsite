namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCinemaModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cinemas",
                c => new
                    {
                        CinemaId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        SeatsLayout = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CinemaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cinemas");
        }
    }
}
