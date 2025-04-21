namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bildirimeklendi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bildirimlers",
                c => new
                    {
                        BildirimID = c.Int(nullable: false, identity: true),
                        MesajID = c.Int(nullable: false),
                        KullaniciID = c.Int(nullable: false),
                        Mesaj = c.String(),
                        Durum = c.Int(nullable: false),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BildirimID)
                .ForeignKey("dbo.EtkinlikMesajs", t => t.MesajID, cascadeDelete: true)
                .ForeignKey("dbo.Kullanicilars", t => t.KullaniciID, cascadeDelete: true)
                .Index(t => t.MesajID)
                .Index(t => t.KullaniciID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bildirimlers", "KullaniciID", "dbo.Kullanicilars");
            DropForeignKey("dbo.Bildirimlers", "MesajID", "dbo.EtkinlikMesajs");
            DropIndex("dbo.Bildirimlers", new[] { "KullaniciID" });
            DropIndex("dbo.Bildirimlers", new[] { "MesajID" });
            DropTable("dbo.Bildirimlers");
        }
    }
}
