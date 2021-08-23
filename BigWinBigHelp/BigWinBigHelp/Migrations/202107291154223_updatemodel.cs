namespace BigWinBigHelp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "multiplier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "multiplier");
        }
    }
}
