using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Finance.Data.Models;

namespace Finance.Data
{
    public class FinanceDbContext : IdentityDbContext<User>
    {
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<LoanStatus> LoanStatuses { get; set; }
        public virtual DbSet<SupportRequest> SupportRequests { get; set; }

        public DbSet<PushSubscription> PushSubscriptions { get; set; }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PushSubscription>(entity =>
            {
                entity.HasKey(e => new { e.PushSubscriptionId, e.UserDeteilId });

                entity.Property(e => e.PushSubscriptionId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.UserDeteil)
                    .WithMany(p => p.PushSubscriptions)
                    .HasForeignKey(d => d.UserDeteilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PushSubscription_UserDetail");
            });
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankAccount_Currency");

                entity.HasOne(d => d.UserDetail)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.UserDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankAccount_UserDetail");
            });
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loan_Currency");

                entity.HasOne(d => d.ContactDetail)
                   .WithMany(p => p.LoanContactDetails)
                   .HasForeignKey(d => d.ContactDetailId)
                   .HasConstraintName("FK_Loan_ContactDetail");

                entity.HasOne(d => d.UserDetail)
                    .WithMany(p => p.LoanUserDetails)
                    .HasForeignKey(d => d.UserDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loan_UserDetail");

                entity.HasOne(d => d.LoanStatus)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.LoanStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loan_LoanStatus");
            });

            modelBuilder.Entity<LoanStatus>()
               .HasData
               (
                   new LoanStatus { LoanStatusId = 1, Name = "Requested"},
                   new LoanStatus { LoanStatusId = 2, Name = "Approved" },
                   new LoanStatus { LoanStatusId = 3, Name = "Rejected" },
                   new LoanStatus { LoanStatusId = 4, Name = "Private" }
                ); 

           modelBuilder.Entity<Currency>()
                .HasData
                (
                    new Currency { CurrencyId = 1, CurrencyName = "USD", IsPopular = true },
                    new Currency { CurrencyId = 2, CurrencyName = "EUR", IsPopular = true },
                    new Currency { CurrencyId = 3, CurrencyName = "UAH", IsPopular = true },
                    new Currency { CurrencyId = 4, CurrencyName = "JPY", IsPopular = false },
                    new Currency { CurrencyId = 5, CurrencyName = "GBP", IsPopular = true },
                    new Currency { CurrencyId = 6, CurrencyName = "CNY", IsPopular = false },
                    new Currency { CurrencyId = 7, CurrencyName = "AUD", IsPopular = false },
                    new Currency { CurrencyId = 8, CurrencyName = "CAD", IsPopular = true },
                    new Currency { CurrencyId = 9, CurrencyName = "CHF", IsPopular = false },
                    new Currency { CurrencyId = 10, CurrencyName = "HKD", IsPopular = false },
                    new Currency { CurrencyId = 11, CurrencyName = "SGD", IsPopular = false },
                    new Currency { CurrencyId = 12, CurrencyName = "SEK", IsPopular = false },
                    new Currency { CurrencyId = 13, CurrencyName = "KRW", IsPopular = false },
                    new Currency { CurrencyId = 14, CurrencyName = "NOK", IsPopular = false },
                    new Currency { CurrencyId = 15, CurrencyName = "NZD", IsPopular = false },
                    new Currency { CurrencyId = 16, CurrencyName = "INR", IsPopular = false },
                    new Currency { CurrencyId = 17, CurrencyName = "MXN", IsPopular = false },
                    new Currency { CurrencyId = 18, CurrencyName = "TWD", IsPopular = false },
                    new Currency { CurrencyId = 19, CurrencyName = "ZAR", IsPopular = false },
                    new Currency { CurrencyId = 20, CurrencyName = "BRL", IsPopular = false },
                    new Currency { CurrencyId = 21, CurrencyName = "DKK", IsPopular = false },
                    new Currency { CurrencyId = 22, CurrencyName = "PLN", IsPopular = false },
                    new Currency { CurrencyId = 23, CurrencyName = "THB", IsPopular = false },
                    new Currency { CurrencyId = 24, CurrencyName = "ILS", IsPopular = false },
                    new Currency { CurrencyId = 25, CurrencyName = "IDR", IsPopular = false },
                    new Currency { CurrencyId = 26, CurrencyName = "CZK", IsPopular = false },
                    new Currency { CurrencyId = 27, CurrencyName = "AED", IsPopular = false },
                    new Currency { CurrencyId = 28, CurrencyName = "TRY", IsPopular = false },
                    new Currency { CurrencyId = 29, CurrencyName = "HUF", IsPopular = false },
                    new Currency { CurrencyId = 30, CurrencyName = "CLP", IsPopular = false },
                    new Currency { CurrencyId = 31, CurrencyName = "SAR", IsPopular = false },
                    new Currency { CurrencyId = 32, CurrencyName = "PHP", IsPopular = false },
                    new Currency { CurrencyId = 33, CurrencyName = "MYR", IsPopular = false },
                    new Currency { CurrencyId = 34, CurrencyName = "COP", IsPopular = false },
                    new Currency { CurrencyId = 35, CurrencyName = "RUB", IsPopular = false },
                    new Currency { CurrencyId = 36, CurrencyName = "RON", IsPopular = false }
                );

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.BankAccount)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BankAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_BankAccount");

                entity.HasOne(d => d.Loan)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.LoanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Loan");

            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.Property(e => e.UserId)
                     .HasDefaultValue(string.Empty);

                entity.HasMany(d => d.Currencies)
                    .WithMany(p => p.UserDetails)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserCurrency",
                        l => l.HasOne<Currency>().WithMany().HasForeignKey("CurrencyId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserCurrency_Currency"),
                        r => r.HasOne<UserDetail>().WithMany().HasForeignKey("UserDetailId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserCurrency_UserDetail"),
                        j =>
                        {
                            j.HasKey("UserDetailId", "CurrencyId");

                            j.ToTable("UserCurrency");
                        });
            });

            modelBuilder.Entity<SupportRequest>(entity =>
            {
                entity.HasOne(d => d.UserDetail)
                    .WithMany(p => p.SupportRequests)
                    .HasForeignKey(d => d.UserDetailId)
                    .HasConstraintName("FK_SupportRequest_UserDetail");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}