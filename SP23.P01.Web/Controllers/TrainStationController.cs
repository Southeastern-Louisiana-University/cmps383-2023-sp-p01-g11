using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SP23.P01.Web.Data;
using SP23.P01.Web.Entities;
using static SP23.P01.Web.Entities.TrainStation;

namespace SP23.P01.Web.Controllers
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
            _dataContext = dataContext;
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
        public ActionResult<TrainStation.TrainStationDto> GetbyId(int id)
        {
            var trains = _dataContext.Set<TrainStation>();
            return base.Ok(trains.Where(x => x.Id == id).Select(x => new TrainStation.TrainStationDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address
            }));

        }

        [HttpPost]
        [Route("/api/station")]
        public ActionResult<TrainStation.TrainStationDto> createTrainStation(TrainStation.TrainStationDto TrainStationCreateDto)
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
        private static bool IsInvalid(TrainStation.TrainStationDto dto)
        {
            return string.IsNullOrWhiteSpace(dto.Name) ||
                   dto.Name.Length > 120 ||
                   string.IsNullOrWhiteSpace(dto.Address);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {

            var trainStationToDelete = _dataContext
            .Set<TrainStation>()
            .FirstOrDefault(x => x.Id == id);

            if (trainStationToDelete == null)
            {
                return Ok(Response);
            }
            _dataContext.SaveChanges();
            return Ok(Response);
        }


    }
}

