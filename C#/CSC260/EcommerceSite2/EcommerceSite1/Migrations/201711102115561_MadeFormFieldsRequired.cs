namespace EcommerceSite1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeFormFieldsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Game", "ImageURL", c => c.String(nullable: false));
            AlterColumn("dbo.Game", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Game", "ShortDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Game", "LongDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Game", "LongDescription", c => c.String());
            AlterColumn("dbo.Game", "ShortDescription", c => c.String());
            AlterColumn("dbo.Game", "Title", c => c.String());
            AlterColumn("dbo.Game", "ImageURL", c => c.String());
        }
    }
}
