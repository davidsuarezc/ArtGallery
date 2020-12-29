namespace GaleryArt.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class galeryDbContext : DbContext
    {
        public galeryDbContext()
            : base("name=galeryDbContext")
        {
        }

        public virtual DbSet<Manifestacione> Manifestaciones { get; set; }
        public virtual DbSet<Obra> Obras { get; set; }
        public virtual DbSet<Premio> Premios { get; set; }
        public virtual DbSet<Tecnica> Tecnicas { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manifestacione>()
                .HasMany(e => e.Obras)
                .WithOptional(e => e.Manifestacione)
                .HasForeignKey(e => e.manifestacion);

            modelBuilder.Entity<Manifestacione>()
                .HasMany(e => e.Tecnicas)
                .WithRequired(e => e.Manifestacione)
                .HasForeignKey(e => e.manifestacion);

            modelBuilder.Entity<Obra>()
                .Property(e => e.precio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Obra>()
                .Property(e => e.path_foto)
                .IsUnicode(false);

            modelBuilder.Entity<Obra>()
                .HasMany(e => e.Premios)
                .WithMany(e => e.Obras)
                .Map(m => m.ToTable("Obras_Premios").MapLeftKey("obra").MapRightKey("premio"));

            modelBuilder.Entity<Tecnica>()
                .HasMany(e => e.Obras)
                .WithOptional(e => e.Tecnica1)
                .HasForeignKey(e => e.tecnica);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
