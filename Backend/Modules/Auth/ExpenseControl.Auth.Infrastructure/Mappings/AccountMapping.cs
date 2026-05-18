using ExpenseControl.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Auth.Infrastructure.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(a => a.Members)
               .WithOne()
               .HasForeignKey(m => m.AccountId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
