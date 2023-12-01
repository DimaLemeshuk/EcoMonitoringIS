using System;
using System.Collections.Generic;
using System.Data;
using System.Printing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto;

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
    public virtual DbSet<Loss> Losses { get; set; }
    public virtual DbSet<Tax> Taxes { get; set; }


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
            entity.Property(e => e.SF).HasColumnName("SF");
            entity.Property(e => e.TaxRate).HasColumnName("taxRate");
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
            entity.Property(e => e.Year);

            entity.Property(e => e.EnterpriseId).HasColumnName("enterprise_id");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.PollutantId).HasColumnName("pollutant_id");
            entity.Property(e => e.ValueMfr).HasColumnName("value_mfr");
            entity.Property(e => e.Concentration).HasColumnName("concentration");
            entity.Property(e => e.HQ).HasColumnName("HQ");
            entity.Property(e => e.rating)
                .HasMaxLength(100)
                .HasColumnName("rating");


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
            entity.Property(e => e.PCR).HasColumnName("PCR");
            entity.Property(e => e.CR).HasColumnName("CR");
            entity.Property(e => e.LADD).HasColumnName("LADD");
            entity.Property(e => e.PollutionId).HasColumnName("pollution_id");
            entity.Property(e => e.Ca).HasColumnName("Ca");
            entity.Property(e => e.Ch).HasColumnName("Ch");
            entity.Property(e => e.Tout).HasColumnName("Tout");
            entity.Property(e => e.Tin).HasColumnName("Tin");
            entity.Property(e => e.Vout).HasColumnName("Vout");
            entity.Property(e => e.Vin).HasColumnName("Vin");
            entity.Property(e => e.EF).HasColumnName("EF");
            entity.Property(e => e.ED).HasColumnName("ED");
            entity.Property(e => e.BW).HasColumnName("BW");
            entity.Property(e => e.AT).HasColumnName("AT");
            entity.Property(e => e.POP).HasColumnName("POP");

            entity.HasOne(d => d.Pollution).WithOne(p => p.Result)
                .HasForeignKey<Result>(d => d.PollutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll2_id");
        });

        modelBuilder.Entity<Loss>(entity =>
        {
            entity.HasKey(e => e.idLoss).HasName("PRIMARY");

            entity.ToTable("losses");
            entity.HasIndex(e => e.pollution_id, "pollution_id");

            entity.Property(e => e.pollution_id).HasColumnName("pollution_id");
            entity.Property(e => e.idLoss).HasColumnName("idLoss");
            entity.Property(e => e.LossUAH).HasColumnName("LossUAH");
            entity.Property(e => e.Mi).HasColumnName("Mi");
            entity.Property(e => e.P).HasColumnName("P");
            entity.Property(e => e.Ai).HasColumnName("Ai");
            entity.Property(e => e.Kpop).HasColumnName("Kpop");
            entity.Property(e => e.Kf).HasColumnName("Kf");
            entity.Property(e => e.Kzi).HasColumnName("Kzi");
            entity.Property(e => e.Qv).HasColumnName("Qv");
            entity.Property(e => e.T).HasColumnName("T");
            entity.Property(e => e.POP_).HasColumnName("POP_");

            entity.HasOne(d => d.Pollution).WithMany(p => p.Losses)
                .HasForeignKey(d => d.pollution_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pollution_id");
        });

  //      CREATE TABLE `ecomonitoringdb`.`taxes` (
  //`idtax` INT NOT NULL AUTO_INCREMENT,
  //`pollution_id` INT NOT NULL,
  //`rate` DOUBLE NOT NULL,
  //`taxUAH` DOUBLE NOT NULL,
  //PRIMARY KEY(`idtax`),
  //INDEX `pollution_id_idx` (`pollution_id` ASC) VISIBLE,
  //CONSTRAINT `tax_pollution_id`
  //  FOREIGN KEY(`pollution_id`)
  //  REFERENCES `ecomonitoringdb`.`pollution` (`idpollution`)
  //  ON DELETE NO ACTION
  //  ON UPDATE NO ACTION);


        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.Idtax).HasName("PRIMARY");

            entity.ToTable("taxes");
            entity.HasIndex(e => e.PollutionId, "pollution_id");

            entity.Property(e => e.PollutionId).HasColumnName("pollution_id");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.TaxUAH).HasColumnName("taxUAH");


            entity.HasOne(d => d.Pollution).WithMany(p => p.Taxes)
                .HasForeignKey(d => d.PollutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pollution_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
