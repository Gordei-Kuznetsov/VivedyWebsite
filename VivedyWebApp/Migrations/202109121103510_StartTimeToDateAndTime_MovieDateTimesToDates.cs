namespace VivedyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartTimeToDateAndTime_MovieDateTimesToDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Screenings", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Screenings", "StartTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Screenings", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Screenings", "StartDate");
        }
    }
}
