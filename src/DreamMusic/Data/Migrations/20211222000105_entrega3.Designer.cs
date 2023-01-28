﻿// <auto-generated />
using System;
using DreamMusic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DreamMusic.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211222000105_entrega3")]
    partial class entrega3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DreamMusic.Models.CompraProveedor", b =>
                {
                    b.Property<int>("CompraProveedorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdministradorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCompraProveedor")
                        .HasColumnType("datetime2");

                    b.Property<double>("PrecioTotal")
                        .HasColumnType("float");

                    b.Property<int?>("ProveedorId")
                        .HasColumnType("int");

                    b.HasKey("CompraProveedorId");

                    b.HasIndex("AdministradorId");

                    b.HasIndex("ProveedorId");

                    b.ToTable("CompraProveedores");
                });

            modelBuilder.Entity("DreamMusic.Models.Comprar", b =>
                {
                    b.Property<int>("CompraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdministradorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClienteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MetodoDePagoID")
                        .HasColumnType("int");

                    b.Property<double>("PrecioTotal")
                        .HasColumnType("float");

                    b.HasKey("CompraId");

                    b.HasIndex("AdministradorId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("MetodoDePagoID");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("DreamMusic.Models.Devolucion", b =>
                {
                    b.Property<int>("DevolucionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClienteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaDevolucion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaEntrega")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MetodoDePagoID")
                        .HasColumnType("int");

                    b.Property<double>("PrecioTotal")
                        .HasColumnType("float");

                    b.HasKey("DevolucionId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("MetodoDePagoID");

                    b.ToTable("Devoluciones");
                });

            modelBuilder.Entity("DreamMusic.Models.Disco", b =>
                {
                    b.Property<int>("DiscoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Artista")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CantidadCompra")
                        .HasColumnType("int");

                    b.Property<int>("CantidadDevolucion")
                        .HasColumnType("int");

                    b.Property<int>("CantidadRestauracion")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaLanzamiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("GeneroID")
                        .HasColumnType("int");

                    b.Property<int>("PrecioComprarProveedor")
                        .HasColumnType("int");

                    b.Property<int>("PrecioDeCompra")
                        .HasColumnType("int");

                    b.Property<int>("PrecioDeDevolucion")
                        .HasColumnType("int");

                    b.Property<int>("PrecioDeRestauracion")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("DiscoId");

                    b.HasAlternateKey("Titulo");

                    b.HasIndex("GeneroID");

                    b.ToTable("Discos");
                });

            modelBuilder.Entity("DreamMusic.Models.Genero", b =>
                {
                    b.Property<int>("GeneroID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GeneroID");

                    b.HasAlternateKey("Nombre");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemCompraProveedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadComprarProveedor")
                        .HasColumnType("int");

                    b.Property<int>("CompraProveedorId")
                        .HasColumnType("int");

                    b.Property<int>("DiscoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompraProveedorId");

                    b.HasIndex("DiscoId");

                    b.ToTable("ItemCompraProveedores");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemDeCompra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadCompra")
                        .HasColumnType("int");

                    b.Property<int>("CompraId")
                        .HasColumnType("int");

                    b.Property<int>("DiscoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompraId");

                    b.HasIndex("DiscoId");

                    b.ToTable("ItemDeCompras");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemDevolucion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadDevolucion")
                        .HasColumnType("int");

                    b.Property<int>("DevolucionId")
                        .HasColumnType("int");

                    b.Property<int>("DiscoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DevolucionId");

                    b.HasIndex("DiscoId");

                    b.ToTable("ItemDevoluciones");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemRestauracion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadRestauracion")
                        .HasColumnType("int");

                    b.Property<int>("DiscoId")
                        .HasColumnType("int");

                    b.Property<int>("RestauracionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscoId", "RestauracionId");

                    b.HasIndex("RestauracionId");

                    b.ToTable("ItemRestauracion");
                });

            modelBuilder.Entity("DreamMusic.Models.MetodoDePago", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PaymentMethod_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("MetodoDePagos");

                    b.HasDiscriminator<string>("PaymentMethod_type").HasValue("PaymentMethod");
                });

            modelBuilder.Entity("DreamMusic.Models.Proveedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("DreamMusic.Models.Restauracion", b =>
                {
                    b.Property<int>("RestauracionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdministradorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaRestauracion")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MetodoDePagoID")
                        .HasColumnType("int");

                    b.Property<double>("PrecioTotal")
                        .HasColumnType("float");

                    b.HasKey("RestauracionId");

                    b.HasIndex("AdministradorId");

                    b.HasIndex("MetodoDePagoID");

                    b.ToTable("restauracion");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DreamMusic.Models.PayPal", b =>
                {
                    b.HasBaseType("DreamMusic.Models.MetodoDePago");

                    b.Property<string>("CorreoElectronico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumTelefono")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefijo")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("PaymentMethod_PayPal");
                });

            modelBuilder.Entity("DreamMusic.Models.TarjetaDeCredito", b =>
                {
                    b.HasBaseType("DreamMusic.Models.MetodoDePago");

                    b.Property<string>("CCV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCaducidad")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumeroTarjeta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("PaymentMethod_CreditCard");
                });

            modelBuilder.Entity("DreamMusic.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Apellido1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Apellido2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("DreamMusic.Models.Administrador", b =>
                {
                    b.HasBaseType("DreamMusic.Models.ApplicationUser");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Administrador");
                });

            modelBuilder.Entity("DreamMusic.Models.Cliente", b =>
                {
                    b.HasBaseType("DreamMusic.Models.ApplicationUser");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Cliente_Nombre");

                    b.HasDiscriminator().HasValue("Cliente");
                });

            modelBuilder.Entity("DreamMusic.Models.CompraProveedor", b =>
                {
                    b.HasOne("DreamMusic.Models.Administrador", "Administrador")
                        .WithMany("CompraProveedores")
                        .HasForeignKey("AdministradorId");

                    b.HasOne("DreamMusic.Models.Proveedor", null)
                        .WithMany("CompraProveedores")
                        .HasForeignKey("ProveedorId");

                    b.Navigation("Administrador");
                });

            modelBuilder.Entity("DreamMusic.Models.Comprar", b =>
                {
                    b.HasOne("DreamMusic.Models.Administrador", null)
                        .WithMany("Compras")
                        .HasForeignKey("AdministradorId");

                    b.HasOne("DreamMusic.Models.Cliente", "Cliente")
                        .WithMany("Compras")
                        .HasForeignKey("ClienteId");

                    b.HasOne("DreamMusic.Models.MetodoDePago", "MetodoDePago")
                        .WithMany()
                        .HasForeignKey("MetodoDePagoID");

                    b.Navigation("Cliente");

                    b.Navigation("MetodoDePago");
                });

            modelBuilder.Entity("DreamMusic.Models.Devolucion", b =>
                {
                    b.HasOne("DreamMusic.Models.Cliente", "Cliente")
                        .WithMany("Devoluciones")
                        .HasForeignKey("ClienteId");

                    b.HasOne("DreamMusic.Models.MetodoDePago", "MetodoDePago")
                        .WithMany()
                        .HasForeignKey("MetodoDePagoID");

                    b.Navigation("Cliente");

                    b.Navigation("MetodoDePago");
                });

            modelBuilder.Entity("DreamMusic.Models.Disco", b =>
                {
                    b.HasOne("DreamMusic.Models.Genero", "Genero")
                        .WithMany("Discos")
                        .HasForeignKey("GeneroID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genero");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemCompraProveedor", b =>
                {
                    b.HasOne("DreamMusic.Models.CompraProveedor", "CompraProveedor")
                        .WithMany("ItemCompraProveedor")
                        .HasForeignKey("CompraProveedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamMusic.Models.Disco", "Disco")
                        .WithMany("DiscosCompradosProveedor")
                        .HasForeignKey("DiscoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompraProveedor");

                    b.Navigation("Disco");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemDeCompra", b =>
                {
                    b.HasOne("DreamMusic.Models.Comprar", "Comprar")
                        .WithMany("ComprarItem")
                        .HasForeignKey("CompraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamMusic.Models.Disco", "Disco")
                        .WithMany("ItemCompra")
                        .HasForeignKey("DiscoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comprar");

                    b.Navigation("Disco");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemDevolucion", b =>
                {
                    b.HasOne("DreamMusic.Models.Devolucion", "Devolucion")
                        .WithMany("ItemDevolucions")
                        .HasForeignKey("DevolucionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamMusic.Models.Disco", "Disco")
                        .WithMany("ItemDevolucion")
                        .HasForeignKey("DiscoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Devolucion");

                    b.Navigation("Disco");
                });

            modelBuilder.Entity("DreamMusic.Models.ItemRestauracion", b =>
                {
                    b.HasOne("DreamMusic.Models.Disco", "Disco")
                        .WithMany("ItemRestauracion")
                        .HasForeignKey("DiscoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamMusic.Models.Restauracion", "Restauracion")
                        .WithMany("ItemRestauracion")
                        .HasForeignKey("RestauracionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disco");

                    b.Navigation("Restauracion");
                });

            modelBuilder.Entity("DreamMusic.Models.Restauracion", b =>
                {
                    b.HasOne("DreamMusic.Models.Administrador", "Administrador")
                        .WithMany("Restauraciones")
                        .HasForeignKey("AdministradorId");

                    b.HasOne("DreamMusic.Models.MetodoDePago", "MetodoDePago")
                        .WithMany()
                        .HasForeignKey("MetodoDePagoID");

                    b.Navigation("Administrador");

                    b.Navigation("MetodoDePago");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DreamMusic.Models.CompraProveedor", b =>
                {
                    b.Navigation("ItemCompraProveedor");
                });

            modelBuilder.Entity("DreamMusic.Models.Comprar", b =>
                {
                    b.Navigation("ComprarItem");
                });

            modelBuilder.Entity("DreamMusic.Models.Devolucion", b =>
                {
                    b.Navigation("ItemDevolucions");
                });

            modelBuilder.Entity("DreamMusic.Models.Disco", b =>
                {
                    b.Navigation("DiscosCompradosProveedor");

                    b.Navigation("ItemCompra");

                    b.Navigation("ItemDevolucion");

                    b.Navigation("ItemRestauracion");
                });

            modelBuilder.Entity("DreamMusic.Models.Genero", b =>
                {
                    b.Navigation("Discos");
                });

            modelBuilder.Entity("DreamMusic.Models.Proveedor", b =>
                {
                    b.Navigation("CompraProveedores");
                });

            modelBuilder.Entity("DreamMusic.Models.Restauracion", b =>
                {
                    b.Navigation("ItemRestauracion");
                });

            modelBuilder.Entity("DreamMusic.Models.Administrador", b =>
                {
                    b.Navigation("CompraProveedores");

                    b.Navigation("Compras");

                    b.Navigation("Restauraciones");
                });

            modelBuilder.Entity("DreamMusic.Models.Cliente", b =>
                {
                    b.Navigation("Compras");

                    b.Navigation("Devoluciones");
                });
#pragma warning restore 612, 618
        }
    }
}