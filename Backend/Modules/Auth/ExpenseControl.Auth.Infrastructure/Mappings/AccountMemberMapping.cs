using ExpenseControl.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Auth.Infrastructure.Mappings;

public class AccountMemberMapping : IEntityTypeConfiguration<AccountMember>
{
    public void Configure(EntityTypeBuilder<AccountMember> builder)
    {
        builder.ToTable("AccountMembers");
        builder.HasKey(m => new { m.AccountId, m.UserId });
        builder.Property(m => m.Role).IsRequired();
    }
}
