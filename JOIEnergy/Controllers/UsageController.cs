namespace JOIEnergy.Controllers
{
  using System;
  using Microsoft.AspNetCore.Mvc;

  [Route("usage")]
  public class UsageController : Controller
  {
    [HttpGet("{smartMeterIdParam}/usage/{startDateParam}")]
    public ObjectResult GetUsage(string smartMeterIdParam, DateTime startDateParam)
    {
      return NotFound(null);
    }
  }
}