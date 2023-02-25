
using Microsoft.EntityFrameworkCore;
using Livraria.Models;

namespace Livraria.Context;

class LivrariaApiContext : DbContext 
{
    public LivrariaApiContext(DbContextOptions<LivrariaApiContext> options):base(options)
    {
        
    }

    public DbSet<Livro> Livros { get; set; }
}