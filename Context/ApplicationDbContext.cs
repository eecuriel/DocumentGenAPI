using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyExpManAPI.Entities;
using MyExpManAPI.Models;

namespace MyExpManAPI.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }

        public DbSet<Concept> Concepts {get; set;}
        public DbSet<Currency> Currencies {get; set;}
        public DbSet<DocumentDetail> DocumentDetails {get; set;}
        public DbSet<DocumentHeader> DocumentHeaders {get; set;}
        public DbSet<DocumentIncome> DocumentIncomes {get; set;}
        public DbSet<Frequency> Frequencies {get; set;}
        public DbSet<ExpenseLog> ExpenseLogs {get; set;}
        public DbSet<UserAditionalData> UserGenerals {get; set;}
        
    }
}