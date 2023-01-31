using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static SP23.P01.Web.TrainStation;

namespace SP23.P01.Web
{
    [ApiController]
    [Route("api/station")]
    public class TrainStationController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TrainStation> trainStations;

        private DataContext _dataContext;
        public TrainStationController(DataContext dataContext)
        {
            this._dataContext = dataContext;
            trainStations = dataContext.Set<TrainStation>();
        }

        [HttpGet]
        public ActionResult<TrainStation[]> Get()
        {
            var trains = _dataContext.Set<TrainStation>();
            return Ok(trains.Select(x => new TrainStation
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
            }));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<TrainStationDto> GetbyId(int id)
        {
            var trains = _dataContext.Set<TrainStation>();
            return Ok(trains.Where(x => x.Id == id).Select(x => new TrainStationDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address
            }));

        }

        [HttpPost]
        [Route("/api/station")]
        public ActionResult<TrainStationDto> createTrainStation(TrainStationDto TrainStationCreateDto)
        {
            if (IsInvalid(TrainStationCreateDto))
            {
                return BadRequest();
            }

            var trains = _dataContext.Set<TrainStation>();

            var trainStationToAdd = new TrainStation
            {
                Name = TrainStationCreateDto.Name,
                Address = TrainStationCreateDto.Address,
            };

            _dataContext.TrainStations.Add(trainStationToAdd);
            _dataContext.SaveChanges();

            TrainStationCreateDto.Id = trainStationToAdd.Id;

            var trainStationToReturn = new TrainStationGetDto
            {
                Name = trainStationToAdd.Name,
                Address = trainStationToAdd.Address,
            };


            return CreatedAtAction
                (nameof(GetbyId),
                new { id = TrainStationCreateDto.Id },
                TrainStationCreateDto);

        }
        private static bool IsInvalid(TrainStationDto dto)
        {
            return string.IsNullOrWhiteSpace(dto.Name) ||
                   dto.Name.Length > 120 ||
                   string.IsNullOrWhiteSpace(dto.Address);
        }


    }
    }

