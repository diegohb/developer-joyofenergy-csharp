// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
  using System.Linq;
  using Microsoft.AspNetCore.Mvc;
  using Newtonsoft.Json.Linq;
  using Services;

  [Route("price-plans")]
  public class PricePlanComparatorController : Controller
  {
    private readonly IAccountService _accountService;
    private readonly IPricePlanService _pricePlanService;

    public PricePlanComparatorController(IPricePlanService pricePlanServiceParam, IAccountService accountServiceParam)
    {
      _pricePlanService = pricePlanServiceParam;
      _accountService = accountServiceParam;
    }

    [HttpGet("compare-all/{smartMeterIdParam}")]
    public ObjectResult CalculatedCostForEachPricePlan(string smartMeterIdParam)
    {
      var pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterIdParam);
      var costPerPricePlan =
        _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterIdParam);
      if (!costPerPricePlan.Any())
      {
        return new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterIdParam));
      }

      dynamic response = JObject.FromObject(costPerPricePlan);

      return
        costPerPricePlan.Any()
          ? new ObjectResult(response)
          : new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterIdParam));
    }

    [HttpGet("recommend/{smartMeterIdParam}")]
    public ObjectResult RecommendCheapestPricePlans(string smartMeterIdParam, int? limitParam = null)
    {
      var consumptionForPricePlans =
        _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterIdParam);

      if (!consumptionForPricePlans.Any())
      {
        return new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterIdParam));
      }

      var recommendations = consumptionForPricePlans.OrderBy(pricePlanComparison => pricePlanComparison.Value);

      if (limitParam.HasValue && limitParam.Value < recommendations.Count())
      {
        return new ObjectResult(recommendations.Take(limitParam.Value));
      }

      return new ObjectResult(recommendations);
    }
  }
}