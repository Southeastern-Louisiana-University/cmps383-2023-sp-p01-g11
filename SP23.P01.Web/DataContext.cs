using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static SP23.P01.Web.TrainStation;

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

            modelBuilder.ApplyConfiguration(new TrainStationConfiguration());
        }



    }
}
