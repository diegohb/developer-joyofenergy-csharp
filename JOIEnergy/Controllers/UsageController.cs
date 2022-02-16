namespace JOIEnergy.Controllers
{
  using System;
  using Microsoft.AspNetCore.Mvc;
  using Services;

  [Route("usage")]
  public class UsageController : Controller
  {
    private readonly IMeterReadingService _meterReadingService;

    public UsageController(IMeterReadingService meterReadingServiceParam)
    {
      _meterReadingService = meterReadingServiceParam;
    }

    [HttpGet("{smartMeterIdParam}/usage/{startDateParam}")]
    public ObjectResult GetUsage(string smartMeterIdParam, DateTime startDateParam)
    {
      try
      {
        var cost = _meterReadingService.GetCostOfAWeekOfReadings(smartMeterIdParam, startDateParam);
        return Ok(cost);
      }
      catch (ApplicationException)
      {
        return NotFound(null);
      }
    }
  }
}