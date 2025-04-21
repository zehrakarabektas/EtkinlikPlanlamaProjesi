namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enlemboylamguncellendi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etkinliklers", "Enlem", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.Etkinliklers", "Boylam", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.Kullanicilars", "Enlem", c => c.Decimal(precision: 18, scale: 8));
            AlterColumn("dbo.Kullanicilars", "Boylam", c => c.Decimal(precision: 18, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Kullanicilars", "Boylam", c => c.Double(nullable: false));
            AlterColumn("dbo.Kullanicilars", "Enlem", c => c.Double(nullable: false));
            AlterColumn("dbo.Etkinliklers", "Boylam", c => c.Double(nullable: false));
            AlterColumn("dbo.Etkinliklers", "Enlem", c => c.Double(nullable: false));
        }
    }
}
