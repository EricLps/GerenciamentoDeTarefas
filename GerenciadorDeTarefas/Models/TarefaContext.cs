using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Models
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Titulo)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}