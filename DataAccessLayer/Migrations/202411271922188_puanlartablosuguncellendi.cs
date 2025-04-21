namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class puanlartablosuguncellendi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Puanlars", "PuanTuru", c => c.Int(nullable: false));
            DropColumn("dbo.Puanlars", "KullaniciPuanlar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Puanlars", "KullaniciPuanlar", c => c.Single(nullable: false));
            DropColumn("dbo.Puanlars", "PuanTuru");
        }
    }
}
