namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enlemboylamsildim : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Etkinliklers", "Enlem");
            DropColumn("dbo.Etkinliklers", "Boylam");
            DropColumn("dbo.Kullanicilars", "Enlem");
            DropColumn("dbo.Kullanicilars", "Boylam");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kullanicilars", "Boylam", c => c.Double());
            AddColumn("dbo.Kullanicilars", "Enlem", c => c.Double());
            AddColumn("dbo.Etkinliklers", "Boylam", c => c.Double());
            AddColumn("dbo.Etkinliklers", "Enlem", c => c.Double());
        }
    }
}
