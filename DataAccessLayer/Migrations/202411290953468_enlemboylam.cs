namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enlemboylam : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Etkinliklers", "Enlem", c => c.Double());
            AlterColumn("dbo.Etkinliklers", "Boylam", c => c.Double());
            AlterColumn("dbo.Kullanicilars", "Enlem", c => c.Double());
            AlterColumn("dbo.Kullanicilars", "Boylam", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Kullanicilars", "Boylam", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Kullanicilars", "Enlem", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Etkinliklers", "Boylam", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Etkinliklers", "Enlem", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
