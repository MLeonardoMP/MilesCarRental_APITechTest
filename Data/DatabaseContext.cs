using Microsoft.EntityFrameworkCore;
using MilesCarRentalAPITechTest.Data.Models;
//Definición de contexto para conección con báses de datos.
namespace MilesCarRentalAPITechTest.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() {}

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
        //set de datos de los modelos para interactuar en el controlador
        public DbSet<Car> cars { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<Market> markets { get; set; }
    }
}
