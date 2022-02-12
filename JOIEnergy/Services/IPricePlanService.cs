namespace JOIEnergy.Services
{
  using System.Collections.Generic;

  public interface IPricePlanService
  {
    Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterIdParam);
  }
}