using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

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
            var trains = dataContext.Set<TrainStation>();
            return Ok(trains.Select(x => new TrainStation
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
            }));
        }
    }
}
