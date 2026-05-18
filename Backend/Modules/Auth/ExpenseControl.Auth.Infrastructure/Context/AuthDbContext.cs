using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Auth.Infrastructure.Context;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User>          Users       { get; set; }
    public DbSet<Account>       Accounts    { get; set; }
    public DbSet<AccountMember> Members     { get; set; }
    public DbSet<Invitation>    Invitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new AccountMapping());
        modelBuilder.ApplyConfiguration(new AccountMemberMapping());
        modelBuilder.ApplyConfiguration(new InvitationMapping());
    }
}
