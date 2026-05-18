using ExpenseControl.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Auth.Infrastructure.Mappings;

public class InvitationMapping : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitations");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InvitedEmail).IsRequired().HasMaxLength(300);
        builder.Property(i => i.Token).IsRequired();
        builder.Property(i => i.Status).IsRequired();
        builder.HasIndex(i => i.Token).IsUnique();
    }
}
