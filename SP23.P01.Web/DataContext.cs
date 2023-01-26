using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SP23.P01.Web
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        public DbSet<TrainStation> TrainStations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }



    }
}
