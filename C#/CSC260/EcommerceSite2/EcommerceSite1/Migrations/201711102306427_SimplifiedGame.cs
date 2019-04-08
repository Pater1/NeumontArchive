namespace EcommerceSite1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifiedGame : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Game", new[] { "ID" });
            DropIndex("dbo.Game", new[] { "Title_Spaceless" });
            DropPrimaryKey("dbo.Game");
            AlterColumn("dbo.Game", "Title_Spaceless", c => c.String(nullable: false, maxLength: 255));
            AddPrimaryKey("dbo.Game", "Title_Spaceless");
            CreateIndex("dbo.Game", "Title_Spaceless", unique: true);
            DropColumn("dbo.Game", "ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "ID", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.Game", new[] { "Title_Spaceless" });
            DropPrimaryKey("dbo.Game");
            AlterColumn("dbo.Game", "Title_Spaceless", c => c.String(maxLength: 255));
            AddPrimaryKey("dbo.Game", "ID");
            CreateIndex("dbo.Game", "Title_Spaceless", unique: true);
            CreateIndex("dbo.Game", "ID", unique: true);
        }
    }
}