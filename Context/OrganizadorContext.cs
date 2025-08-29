using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Domain.Models;

namespace TrilhaApiDesafio.Context
{
    public class OrganizadorContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Tarefa> Tarefas { get; set; }
        
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options, IConfiguration configuration) 
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured)
                return;
            var connection = _configuration.GetConnectionString("mysql")?.ToString();
            if(string.IsNullOrEmpty(connection))
                return;
            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        }

        
    }
}