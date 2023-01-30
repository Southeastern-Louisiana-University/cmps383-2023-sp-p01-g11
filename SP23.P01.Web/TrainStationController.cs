using Azure;
using Microsoft.AspNetCore.Mvc;
using static SP23.P01.Web.TrainStation;

namespace SP23.P01.Web
{
    [ApiController]
    [Route("api/station")]
    public class TrainStationController : ControllerBase
    {
        private DataContext _dataContext;
        public TrainStationController(DataContext dataContext)
        {
            this._dataContext = dataContext;
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

        
        }
    }

