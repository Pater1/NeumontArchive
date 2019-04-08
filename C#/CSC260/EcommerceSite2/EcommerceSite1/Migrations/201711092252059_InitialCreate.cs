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
                        ID = c.Int(nullable: false, identity: true),
                        ImageURL = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Title = c.String(),
                        Link = c.String(),
                        ShortDescription = c.String(),
                        LongDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Game");
        }
    }
}
