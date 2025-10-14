using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Authors.Domain.Aggregates;

namespace Authors.Infrastructure.EntityConfigurations;

/// <summary>
/// Entity Framework configuration for Author aggregate
/// </summary>
public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");

        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .HasColumnName("au_id")
            .HasMaxLength(11)
            .IsRequired();

        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("au_fname")
                .HasMaxLength(20)
                .IsRequired();

            name.Property(n => n.LastName)
                .HasColumnName("au_lname")
                .HasMaxLength(40)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Phone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("phone")
                .HasMaxLength(12)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Address, address =>
        {
            address.Property(ad => ad.Street)
                .HasColumnName("address")
                .HasMaxLength(40);

            address.Property(ad => ad.City)
                .HasColumnName("city")
                .HasMaxLength(20);

            address.Property(ad => ad.State)
                .HasColumnName("state")
                .HasMaxLength(2);

            address.Property(ad => ad.ZipCode)
                .HasColumnName("zip")
                .HasMaxLength(5);
        });

        builder.Property(a => a.HasContract)
            .HasColumnName("contract")
            .IsRequired();

        builder.Ignore(a => a.DomainEvents);
    }
}
