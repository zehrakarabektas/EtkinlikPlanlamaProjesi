namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enlemboylameklendi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Etkinliklers", "enlem", c => c.Double(nullable: false));
            AddColumn("dbo.Etkinliklers", "boylam", c => c.Double(nullable: false));
            AddColumn("dbo.Kullanicilars", "enlem", c => c.Double(nullable: false));
            AddColumn("dbo.Kullanicilars", "boylam", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kullanicilars", "boylam");
            DropColumn("dbo.Kullanicilars", "enlem");
            DropColumn("dbo.Etkinliklers", "boylam");
            DropColumn("dbo.Etkinliklers", "enlem");
        }
    }
}
