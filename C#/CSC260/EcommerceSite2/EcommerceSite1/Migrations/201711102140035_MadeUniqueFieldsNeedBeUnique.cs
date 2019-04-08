namespace EcommerceSite1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeUniqueFieldsNeedBeUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Game", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Game", "Title_Spaceless", c => c.String(maxLength: 255));
            CreateIndex("dbo.Game", "ID", unique: true);
            CreateIndex("dbo.Game", "Title", unique: true);
            CreateIndex("dbo.Game", "Title_Spaceless", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Game", new[] { "Title_Spaceless" });
            DropIndex("dbo.Game", new[] { "Title" });
            DropIndex("dbo.Game", new[] { "ID" });
            AlterColumn("dbo.Game", "Title_Spaceless", c => c.String());
            AlterColumn("dbo.Game", "Title", c => c.String(nullable: false));
        }
    }
}
