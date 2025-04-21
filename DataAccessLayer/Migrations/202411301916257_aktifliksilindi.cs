namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aktifliksilindi : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EtkinlikKategoris", "KategoriDurumu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EtkinlikKategoris", "KategoriDurumu", c => c.Boolean(nullable: false));
        }
    }
}
