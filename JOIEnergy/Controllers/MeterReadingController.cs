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

    public MeterReadingController(IMeterReadingService meterReadingServiceParam)
    {
      _meterReadingService = meterReadingServiceParam;
    }

    [HttpGet("read/{smartMeterIdParam}")]
    public ObjectResult GetReading(string smartMeterIdParam)
    {
      return new OkObjectResult(_meterReadingService.GetReadings(smartMeterIdParam));
    }

    // POST api/values
    [HttpPost("store")]
    public ObjectResult Post([FromBody] MeterReadings meterReadingsParam)
    {
      if (!isMeterReadingsValid(meterReadingsParam))
      {
        return new BadRequestObjectResult("Internal Server Error");
      }

      _meterReadingService.StoreReadings(meterReadingsParam.SmartMeterId, meterReadingsParam.ElectricityReadings);
      return new OkObjectResult("{}");
    }

    #region Support Methods

    private bool isMeterReadingsValid(MeterReadings meterReadingsParam)
    {
      var smartMeterId = meterReadingsParam.SmartMeterId;
      var electricityReadings = meterReadingsParam.ElectricityReadings;
      return smartMeterId != null && smartMeterId.Any()
                                  && electricityReadings != null && electricityReadings.Any();
    }

    #endregion
  }
}