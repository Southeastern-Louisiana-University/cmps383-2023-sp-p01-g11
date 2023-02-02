using Microsoft.EntityFrameworkCore;
using SP23.P01.Web.Entities;

namespace SP23.P01.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            var context = services.GetRequiredService<DataContext>();
            context.Database.Migrate();

            AddTrainStations(context);
        }

        private static void AddTrainStations(DataContext context)
        {
            var products = context.Set<TrainStation>();
            if (products.Any())
            {
                return;
            }

            products.Add(new TrainStation
            {
                Name = "New York station",
                Address = "321 Albany St"
            });
            products.Add(new TrainStation
            {
                Name = "Texas Station",
                Address = "341 Austin Rd"
            });
            products.Add(new TrainStation
            {
                Name = "New Orleans station",
                Address = "504 Nola Blvd"
            });
            context.SaveChanges();
        }
    }
}