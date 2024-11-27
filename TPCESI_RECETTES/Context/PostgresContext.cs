using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Commentaire> Commentaires { get; set; }

    public virtual DbSet<Etape> Etapes { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recette> Recettes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5434;Username=postgres;Password=ilovecesi;Database=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorie_pkey");

            entity.ToTable("categorie");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Commentaire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("commentaire_pkey");

            entity.ToTable("commentaire");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasMany(d => d.IdRecettes).WithMany(p => p.Ids1)
                .UsingEntity<Dictionary<string, object>>(
                    "Posseder",
                    r => r.HasOne<Recette>().WithMany()
                        .HasForeignKey("IdRecette")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("posseder_id_recette_fkey"),
                    l => l.HasOne<Commentaire>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("posseder_id_fkey"),
                    j =>
                    {
                        j.HasKey("Id", "IdRecette").HasName("posseder_pkey");
                        j.ToTable("posseder");
                        j.IndexerProperty<int>("Id").HasColumnName("id");
                        j.IndexerProperty<int>("IdRecette").HasColumnName("id_recette");
                    });
        });

        modelBuilder.Entity<Etape>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("etapes_pkey");

            entity.ToTable("etapes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");

            entity.HasMany(d => d.IdRecettes).WithMany(p => p.IdsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Composer",
                    r => r.HasOne<Recette>().WithMany()
                        .HasForeignKey("IdRecette")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("composer_id_recette_fkey"),
                    l => l.HasOne<Etape>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("composer_id_fkey"),
                    j =>
                    {
                        j.HasKey("Id", "IdRecette").HasName("composer_pkey");
                        j.ToTable("composer");
                        j.IndexerProperty<int>("Id").HasColumnName("id");
                        j.IndexerProperty<int>("IdRecette").HasColumnName("id_recette");
                    });
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredient_pkey");

            entity.ToTable("ingredient");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");

            entity.HasMany(d => d.IdRecettes).WithMany(p => p.Ids)
                .UsingEntity<Dictionary<string, object>>(
                    "Avoir",
                    r => r.HasOne<Recette>().WithMany()
                        .HasForeignKey("IdRecette")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("avoir_id_recette_fkey"),
                    l => l.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("avoir_id_fkey"),
                    j =>
                    {
                        j.HasKey("Id", "IdRecette").HasName("avoir_pkey");
                        j.ToTable("avoir");
                        j.IndexerProperty<int>("Id").HasColumnName("id");
                        j.IndexerProperty<int>("IdRecette").HasColumnName("id_recette");
                    });
        });

        modelBuilder.Entity<Recette>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recette_pkey");

            entity.ToTable("recette");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Difficulte).HasColumnName("difficulte");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("nom");
            entity.Property(e => e.Tempscuisson).HasColumnName("tempscuisson");
            entity.Property(e => e.Tempsprep).HasColumnName("tempsprep");

            entity.HasMany(d => d.IdCategories).WithMany(p => p.Ids)
                .UsingEntity<Dictionary<string, object>>(
                    "Appartenir",
                    r => r.HasOne<Categorie>().WithMany()
                        .HasForeignKey("IdCategorie")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("appartenir_id_categorie_fkey"),
                    l => l.HasOne<Recette>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("appartenir_id_fkey"),
                    j =>
                    {
                        j.HasKey("Id", "IdCategorie").HasName("appartenir_pkey");
                        j.ToTable("appartenir");
                        j.IndexerProperty<int>("Id").HasColumnName("id");
                        j.IndexerProperty<int>("IdCategorie").HasColumnName("id_categorie");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
