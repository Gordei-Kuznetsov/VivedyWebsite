namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChabgedBookingVerTimeToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "VerificationTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "VerificationTime", c => c.String());
        }
    }
}
