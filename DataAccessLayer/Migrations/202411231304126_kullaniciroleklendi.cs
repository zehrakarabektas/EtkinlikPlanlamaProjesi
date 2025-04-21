namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kullaniciroleklendi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kullanicilars", "KullaniciRole", c => c.String(maxLength: 255));
            DropTable("dbo.WebAdmins");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WebAdmins",
                c => new
                    {
                        AdminID = c.Int(nullable: false, identity: true),
                        AdminAdi = c.String(maxLength: 50),
                        Sifre = c.String(maxLength: 255),
                        AdminRole = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.AdminID);
            
            DropColumn("dbo.Kullanicilars", "KullaniciRole");
        }
    }
}
