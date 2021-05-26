namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedBookingModelAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Seats", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Seats");
        }
    }
}
