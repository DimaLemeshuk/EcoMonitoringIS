using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EcoMonitoringIS.Models;

public partial class EcomonitoringdbContext : DbContext
{
    public EcomonitoringdbContext()
    {
    }

    public EcomonitoringdbContext(DbContextOptions<EcomonitoringdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Belonging> Belongings { get; set; }

    public virtual DbSet<Enterprise> Enterprises { get; set; }

    public virtual DbSet<Pollutant> Pollutants { get; set; }

    public virtual DbSet<Pollution> Pollutions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=localhost;user=root;password=12345;database=ecomonitoringdb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Belonging>(entity =>
        {
            entity.HasKey(e => e.Idbelonging).HasName("PRIMARY");

            entity.ToTable("belonging");

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Idbelonging).HasColumnName("idbelonging");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasKey(e => e.Identerprise).HasName("PRIMARY");

            entity.ToTable("enterprise");

            entity.HasIndex(e => e.BelongingId, "enterprise_idx");

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Identerprise).HasColumnName("identerprise");
            entity.Property(e => e.Activity)
                .HasMaxLength(300)
                .HasColumnName("activity");
            entity.Property(e => e.Addres)
                .HasMaxLength(300)
                .HasColumnName("addres");
            entity.Property(e => e.BelongingId).HasColumnName("belonging_id");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");

            entity.HasOne(d => d.Belonging).WithMany(p => p.Enterprises)
                .HasForeignKey(d => d.BelongingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enterprise");
        });

        modelBuilder.Entity<Pollutant>(entity =>
        {
            entity.HasKey(e => e.Idpollutant).HasName("PRIMARY");

            entity.ToTable("pollutant");

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Idpollutant).HasColumnName("idpollutant");
            entity.Property(e => e.DangerClass).HasColumnName("danger_class");
            entity.Property(e => e.Gdk).HasColumnName("gdk");
            entity.Property(e => e.Mfr).HasColumnName("mfr");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Pollution>(entity =>
        {
            entity.HasKey(e => e.Idpollution).HasName("PRIMARY");

            entity.ToTable("pollution");

            entity.HasIndex(e => e.EnterpriseId, "ent_id_idx");

            entity.HasIndex(e => e.PollutantId, "poll_id_idx");

            entity.Property(e => e.Idpollution).HasColumnName("idpollution");
            ////------
            entity.Property(e => e.Year);
            ////---------
            //entity.Property(e => e.Date)
            //    .HasColumnType("date")
            //    .HasColumnName("date");
            entity.Property(e => e.EnterpriseId).HasColumnName("enterprise_id");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.PollutantId).HasColumnName("pollutant_id");
            entity.Property(e => e.ValueMfr).HasColumnName("value_mfr");

            entity.HasOne(d => d.Enterprise).WithMany(p => p.Pollutions)
                .HasForeignKey(d => d.EnterpriseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ent_id");

            entity.HasOne(d => d.Pollutant).WithMany(p => p.Pollutions)
                .HasForeignKey(d => d.PollutantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll_id");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Idresults).HasName("PRIMARY");

            entity.ToTable("results");

            entity.HasIndex(e => e.PollutionId, "pollution_id_UNIQUE").IsUnique();

            entity.Property(e => e.Idresults).HasColumnName("idresults");
            entity.Property(e => e.Exceeding).HasColumnName("exceeding");
            entity.Property(e => e.PollutionId).HasColumnName("pollution_id");

            entity.HasOne(d => d.Pollution).WithOne(p => p.Result)
                .HasForeignKey<Result>(d => d.PollutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll2_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
