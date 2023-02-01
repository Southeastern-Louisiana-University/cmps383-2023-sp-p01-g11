using Microsoft.EntityFrameworkCore;
using SP23.P01.Web.Entities;
using System.Reflection;
using static SP23.P01.Web.Entities.TrainStation;

namespace SP23.P01.Web.Data
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
