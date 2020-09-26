using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DocumentGenAPI.Entities;
using DocumentGenAPI.Models;

namespace DocumentGenAPI.Context
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
        public DbSet<DocumentExtraCharge> DocumentExtraCharges {get; set;}
        public DbSet<Customer> Customers {get; set;}
        public DbSet<ExpenseLog> ExpenseLogs {get; set;}
        public DbSet<UserAditionalData> UserGenerals {get; set;}
        
        public DbSet<DocumentTerm> DocumentTerms {get; set;}
        
    }
}