namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChagedBookingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "UserEmail", c => c.String(nullable: false));
            AddColumn("dbo.Movies", "Description", c => c.String());
            AlterColumn("dbo.Bookings", "RotationId", c => c.String(nullable: false));
            DropColumn("dbo.Bookings", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "UserId", c => c.String());
            AlterColumn("dbo.Bookings", "RotationId", c => c.String());
            DropColumn("dbo.Movies", "Description");
            DropColumn("dbo.Bookings", "UserEmail");
        }
    }
}
