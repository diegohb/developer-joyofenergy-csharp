

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
  using System.Linq;
  using Domain;
  using Microsoft.AspNetCore.Mvc;
  using Services;

  [Route("readings")]
  public class MeterReadingController : Controller
  {
    private readonly IMeterReadingService _meterReadingService;

    public MeterReadingController(IMeterReadingService meterReadingService)
    {
      _meterReadingService = meterReadingService;
    }

    [HttpGet("read/{smartMeterId}")]
    public ObjectResult GetReading(string smartMeterId)
    {
      return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
    }

    // POST api/values
    [HttpPost("store")]
    public ObjectResult Post([FromBody] MeterReadings meterReadings)
    {
      if (!IsMeterReadingsValid(meterReadings))
      {
        return new BadRequestObjectResult("Internal Server Error");
      }

      _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
      return new OkObjectResult("{}");
    }

    #region Support Methods

    private bool IsMeterReadingsValid(MeterReadings meterReadings)
    {
      var smartMeterId = meterReadings.SmartMeterId;
      var electricityReadings = meterReadings.ElectricityReadings;
      return smartMeterId != null && smartMeterId.Any()
                                  && electricityReadings != null && electricityReadings.Any();
    }

    #endregion
  }
}