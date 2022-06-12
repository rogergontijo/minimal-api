using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;

namespace minimal_api.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions options) : base(options)        
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cidade>().HasKey(cidade => cidade.Id);
        }

        public DbSet<Cidade> Cidades { get; set; }        
    }
}