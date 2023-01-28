using DreamMusic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DreamMusic.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Disco> Discos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<ItemCompraProveedor> ItemCompraProveedores { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<CompraProveedor> CompraProveedores { get; set; }
        public DbSet<Comprar> Compras { get; set; }
        public DbSet<ItemDeCompra> ItemDeCompras { get; set; }
        public DbSet<MetodoDePago> MetodoDePagos { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<ItemDevolucion> ItemDevoluciones { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Restauracion> restauracion { get; set; }
        public DbSet<ItemRestauracion> ItemRestauracion { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<MetodoDePago>()
                .HasDiscriminator<string>("PaymentMethod_type")
                .HasValue<MetodoDePago>("PaymentMethod")
                .HasValue<TarjetaDeCredito>("PaymentMethod_CreditCard")
                .HasValue<PayPal>("PaymentMethod_PayPal");

            builder.Entity<Genero>().HasAlternateKey(g => new { g.Nombre });
            builder.Entity<Disco>().HasAlternateKey(m => new { m.Titulo });
            builder.Entity<ItemRestauracion>().HasAlternateKey(pi => new { pi.DiscoId, pi.RestauracionId });
        }
    }
}
