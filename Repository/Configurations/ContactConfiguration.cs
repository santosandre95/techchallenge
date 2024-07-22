using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType(nameof(SqlDbType.UniqueIdentifier)).IsRequired();
            builder.Property(e => e.CreatedDate).HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.Name).HasColumnType("VARCHAR(150)").IsRequired();
            builder.Property(e => e.Email).HasColumnType("VARCHAR(150)");
            builder.Property(e => e.Phone).HasColumnType("VARCHAR(9)").IsRequired();
            builder.Property(e => e.Ddd).HasColumnType("VARCHAR(2)").IsRequired();
        }
    }
}
