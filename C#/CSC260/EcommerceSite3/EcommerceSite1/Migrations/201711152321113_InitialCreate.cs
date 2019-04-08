namespace EcommerceSite1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Title_Spaceless = c.String(nullable: false, maxLength: 255),
                        ImageURL = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Title = c.String(nullable: false, maxLength: 255),
                        Link = c.String(),
                        ShortDescription = c.String(nullable: false),
                        LongDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Title_Spaceless)
                .Index(t => t.Title_Spaceless, unique: true)
                .Index(t => t.Title, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Game", new[] { "Title" });
            DropIndex("dbo.Game", new[] { "Title_Spaceless" });
            DropTable("dbo.Game");
        }
    }
}
