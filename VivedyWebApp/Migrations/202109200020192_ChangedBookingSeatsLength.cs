namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBookingSeatsLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "Seats", c => c.String(nullable: false, maxLength: 48));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "Seats", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
