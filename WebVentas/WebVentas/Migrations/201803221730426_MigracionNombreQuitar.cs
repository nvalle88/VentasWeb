namespace WebVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionNombreQuitar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Nombre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String());
        }
    }
}
