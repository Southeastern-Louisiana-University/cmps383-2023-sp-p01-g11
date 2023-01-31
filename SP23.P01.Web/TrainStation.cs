using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace SP23.P01.Web
{
    public class TrainStation
    {
           public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }


        public class TrainStationDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }

        }

        public class TrainStationGetDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class TrainStationCreateDto
        {
            public string? Name { get; set; }
            public string? Address { get; set; }
        }

        public class TrainStationUpdateDto
        {
            public string? Name { get; set; }
            public string? Address { get; set; }

        }

        public class TrainStationConfiguration : IEntityTypeConfiguration<TrainStation>
            {
            public void Configure(EntityTypeBuilder<TrainStation> builder)
            {
                builder
                    .Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(120);

                builder
                    .Property(x => x.Address)
                    .IsRequired();
            }

        } 

    }
}

