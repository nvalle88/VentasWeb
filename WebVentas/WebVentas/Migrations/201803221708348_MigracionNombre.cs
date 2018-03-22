namespace WebVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionNombre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Nombre");
        }
    }
}
