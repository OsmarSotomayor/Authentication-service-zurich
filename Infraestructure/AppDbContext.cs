using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Nombre completo del usuario");

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(u => u.Rol)
                    .IsRequired()
                    .HasConversion<string>(); // Guarda el enum como string en la DB

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                // Índice para campos únicos
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();
            });
        }
    }
}
