namespace WebVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class camposnuevos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TokenContrasena", c => c.String());
            AddColumn("dbo.AspNetUsers", "Foto", c => c.String());
            AddColumn("dbo.AspNetUsers", "Estado", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Direccion", c => c.String());
            AddColumn("dbo.AspNetUsers", "Identificacion", c => c.String());
            AddColumn("dbo.AspNetUsers", "Nombres", c => c.String());
            AddColumn("dbo.AspNetUsers", "Apellidos", c => c.String());
            AddColumn("dbo.AspNetUsers", "Telefono", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Telefono");
            DropColumn("dbo.AspNetUsers", "Apellidos");
            DropColumn("dbo.AspNetUsers", "Nombres");
            DropColumn("dbo.AspNetUsers", "Identificacion");
            DropColumn("dbo.AspNetUsers", "Direccion");
            DropColumn("dbo.AspNetUsers", "Estado");
            DropColumn("dbo.AspNetUsers", "Foto");
            DropColumn("dbo.AspNetUsers", "TokenContrasena");
        }
    }
}
