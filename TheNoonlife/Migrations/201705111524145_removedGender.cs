namespace TheNoonlife.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedGender : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String(maxLength: 20));
        }
    }
}
