using Microsoft.EntityFrameworkCore;
using SP23.P01.Web.Entities;
using System.Reflection;
using static SP23.P01.Web.Entities.TrainStation;

namespace SP23.P01.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        public DbSet<TrainStation> TrainStations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TrainStationConfiguration());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }



    }
}
