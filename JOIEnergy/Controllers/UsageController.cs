namespace JOIEnergy.Controllers
{
  using System;
  using Microsoft.AspNetCore.Mvc;
  using Services;

  [Route("usage")]
  public class UsageController : Controller
  {
    private readonly UsageService _usageService;

    public UsageController(UsageService usageServiceParam)
    {
      _usageService = usageServiceParam;
    }

    [HttpGet("{smartMeterIdParam}/usage/{startDateParam}")]
    public ObjectResult GetUsage(string smartMeterIdParam, DateTime startDateParam)
    {
      try
      {
        var cost = _usageService.GetCostOfAWeekOfReadings(smartMeterIdParam, startDateParam);
        return Ok(cost);
      }
      catch (ApplicationException)
      {
        return NotFound(null);
      }
    }
  }
}