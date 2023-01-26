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
            public int Address { get; set; }
        }
    }
}
}
