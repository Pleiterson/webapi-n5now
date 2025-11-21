using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5Permissions.Domain.Entities;

namespace N5Permissions.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NombreEmpleado)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ApellidoEmpleado)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.TipoPermiso)
                .IsRequired();

            builder.Property(x => x.FechaPermiso)
                .IsRequired();

            builder.HasOne(x => x.PermissionType)
                .WithMany()
                .HasForeignKey(x => x.TipoPermiso);
        }
    }
}
