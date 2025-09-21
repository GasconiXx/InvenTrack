using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Models;

public partial class InvenTrackContext : DbContext
{
    public InvenTrackContext()
    {
    }

    public InvenTrackContext(DbContextOptions<InvenTrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacene> Almacenes { get; set; }

    public virtual DbSet<Envio> Envios { get; set; }

    public virtual DbSet<EnvioRuta> EnvioRutas { get; set; }

    public virtual DbSet<Incidencia> Incidencias { get; set; }

    public virtual DbSet<MovimientosPaquete> MovimientosPaquetes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<Ruta> Rutas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=InvenTrack;User Id=admin;Password=admin;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacene>(entity =>
        {
            entity.HasKey(e => e.AlmacenId).HasName("PK__Almacene__022A087680FECE16");

            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.EnvioId).HasName("PK__Envios__D024E23FAAA07C2E");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.FechaSalida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Paquete).WithMany(p => p.Envios)
                .HasForeignKey(d => d.PaqueteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Envios__PaqueteI__59063A47");

            entity.HasOne(d => d.Repartidor).WithMany(p => p.Envios)
                .HasForeignKey(d => d.RepartidorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Envios__Repartid__59FA5E80");
        });

        modelBuilder.Entity<EnvioRuta>(entity =>
        {
            entity.HasKey(e => e.EnvioRutaId).HasName("PK__EnvioRut__1F63186C8D40ECAC");

            entity.HasOne(d => d.Envio).WithMany(p => p.EnvioRuta)
                .HasForeignKey(d => d.EnvioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EnvioRuta__Envio__5EBF139D");

            entity.HasOne(d => d.Ruta).WithMany(p => p.EnvioRuta)
                .HasForeignKey(d => d.RutaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EnvioRuta__RutaI__5FB337D6");
        });

        modelBuilder.Entity<Incidencia>(entity =>
        {
            entity.HasKey(e => e.IncidenciaId).HasName("PK__Incidenc__E41133E6AF4D00AB");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Resuelta).HasDefaultValue(false);

            entity.HasOne(d => d.Paquete).WithMany(p => p.Incidencia)
                .HasForeignKey(d => d.PaqueteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incidenci__Paque__6477ECF3");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Incidencia)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incidenci__Usuar__656C112C");
        });

        modelBuilder.Entity<MovimientosPaquete>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__Movimien__BF923C2CEA9AC367");

            entity.ToTable("MovimientosPaquete");

            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observaciones).HasMaxLength(255);
            entity.Property(e => e.TipoMovimiento).HasMaxLength(50);

            entity.HasOne(d => d.Almacen).WithMany(p => p.MovimientosPaquetes)
                .HasForeignKey(d => d.AlmacenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__Almac__534D60F1");

            entity.HasOne(d => d.Paquete).WithMany(p => p.MovimientosPaquetes)
                .HasForeignKey(d => d.PaqueteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__Paque__52593CB8");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MovimientosPaquetes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__Usuar__5441852A");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B6138C080C2F8");

            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__ClienteId__693CA210");

            entity.HasOne(d => d.Paquete).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PaqueteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__PaqueteId__6A30C649");
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.PaqueteId).HasName("PK__Paquetes__7B9F2DB2B785FEC2");

            entity.HasIndex(e => e.CodigoSeguimiento, "UQ__Paquetes__6A8777543C784D9F").IsUnique();

            entity.Property(e => e.CodigoSeguimiento).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Dimensiones).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Peso).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paquetes__Client__4D94879B");
        });

        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.HasKey(e => e.RutaId).HasName("PK__Rutas__7B61998EBB54A610");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B8FD19918E");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534B6CED6CE").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ContraseñaHash).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
