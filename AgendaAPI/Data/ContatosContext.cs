using AgendaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaAPI.Data;

public class ContatosContext : DbContext
{
    public ContatosContext(DbContextOptions<ContatosContext> opts) : base(opts)
    {
        
    }

    public DbSet<Contatos> Contato { get; set; }
}
