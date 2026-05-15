using Cwiczenia7.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia7.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pc> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PcComponent> PCComponents { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(entity =>
        {
            entity.ToTable("PCs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Weight).IsRequired();
            entity.Property(e => e.Warranty).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Stock).IsRequired();
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.ToTable("ComponentManufacturers");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.FoundationDate).HasColumnType("date").IsRequired();
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.ToTable("ComponentTypes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.ToTable("Components");
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code).HasColumnType("char(10)").IsRequired();
            entity.Property(e => e.Name).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Description).IsRequired();

            entity.HasOne(e => e.ComponentManufacturer)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentManufacturerId);

            entity.HasOne(e => e.ComponentType)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentTypeId);
        });

        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.ToTable("PCComponents");
            entity.HasKey(e => new { e.PcId, e.ComponentCode });

            entity.Property(e => e.ComponentCode).HasColumnType("char(10)");
            entity.Property(e => e.Amount).IsRequired();

            entity.HasOne(e => e.Pc)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.PcId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.ComponentCode);
        });
                modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer
            {
                Id = 1,
                Abbreviation = "AMD",
                FullName = "Advanced Micro Devices",
                FoundationDate = new DateTime(1969, 5, 1)
            },
            new ComponentManufacturer
            {
                Id = 2,
                Abbreviation = "NV",
                FullName = "NVIDIA Corporation",
                FoundationDate = new DateTime(1993, 4, 5)
            },
            new ComponentManufacturer
            {
                Id = 3,
                Abbreviation = "COR",
                FullName = "Corsair Gaming Inc.",
                FoundationDate = new DateTime(1994, 1, 1)
            }
        );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType
            {
                Id = 1,
                Abbreviation = "CPU",
                Name = "Processor"
            },
            new ComponentType
            {
                Id = 2,
                Abbreviation = "GPU",
                Name = "Graphics Card"
            },
            new ComponentType
            {
                Id = 3,
                Abbreviation = "RAM",
                Name = "Memory"
            }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU0000001",
                Name = "Ryzen 7 7800X3D",
                Description = "8-core gaming processor",
                ComponentManufacturerId = 1,
                ComponentTypeId = 1
            },
            new Component
            {
                Code = "GPU0000001",
                Name = "RTX 4080 Super",
                Description = "High-end gaming graphics card",
                ComponentManufacturerId = 2,
                ComponentTypeId = 2
            },
            new Component
            {
                Code = "RAM0000001",
                Name = "Corsair DDR5 16GB",
                Description = "DDR5 RAM module",
                ComponentManufacturerId = 3,
                ComponentTypeId = 3
            }
        );

        modelBuilder.Entity<Pc>().HasData(
            new Pc
            {
                Id = 1,
                Name = "Gaming Beast X",
                Weight = 12.5,
                Warranty = 36,
                CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
                Stock = 5
            },
            new Pc
            {
                Id = 2,
                Name = "Office Mini Pro",
                Weight = 4.2,
                Warranty = 24,
                CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0),
                Stock = 12
            },
            new Pc
            {
                Id = 3,
                Name = "Budget Station",
                Weight = 7.1,
                Warranty = 12,
                CreatedAt = new DateTime(2026, 3, 10, 10, 0, 0),
                Stock = 8
            }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent
            {
                PcId = 1,
                ComponentCode = "CPU0000001",
                Amount = 1
            },
            new PcComponent
            {
                PcId = 1,
                ComponentCode = "GPU0000001",
                Amount = 1
            },
            new PcComponent
            {
                PcId = 1,
                ComponentCode = "RAM0000001",
                Amount = 2
            }
        );
    }
}