using Microsoft.EntityFrameworkCore;
using APICitasMedicas.Model;

namespace APICitasMedicas.Context
{

    public class DbContextCitas : DbContext
    {

        public DbContextCitas(DbContextOptions<DbContextCitas> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Citas> Citas { get; set; }

        public DbSet<Procedimientos> Procedimientos { get; set; }
    }
}