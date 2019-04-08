namespace EcommerceSite1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTitle_SpacelessParameter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Title_Spaceless", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "Title_Spaceless");
        }
    }
}
