namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using System.Linq;
  using Domain;

  public class PricePlanService : IPricePlanService
  {
    private readonly IMeterReadingService _meterReadingService;
    private readonly List<PricePlan> _pricePlans;

    public PricePlanService(List<PricePlan> pricePlan, IMeterReadingService meterReadingService)
    {
      _pricePlans = pricePlan;
      _meterReadingService = meterReadingService;
    }

    public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
    {
      var electricityReadings = _meterReadingService.GetReadings(smartMeterId);

      if (!electricityReadings.Any())
      {
        return new Dictionary<string, decimal>();
      }

      return _pricePlans.ToDictionary
      (plan => plan.EnergySupplier.ToString(),
        plan => calculateCost(electricityReadings, plan));
    }

    #region Support Methods

    private decimal calculateAverageReading(List<ElectricityReading> electricityReadings)
    {
      var newSummedReadings = electricityReadings.Select(readings => readings.Reading)
        .Aggregate((reading, accumulator) => reading + accumulator);

      return newSummedReadings / electricityReadings.Count();
    }

    private decimal calculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
    {
      var average = calculateAverageReading(electricityReadings);
      var timeElapsed = calculateTimeElapsed(electricityReadings);
      var averagedCost = average / timeElapsed;
      return averagedCost * pricePlan.UnitRate;
    }

    private decimal calculateTimeElapsed(List<ElectricityReading> electricityReadings)
    {
      var first = electricityReadings.Min(reading => reading.Time);
      var last = electricityReadings.Max(reading => reading.Time);

      return (decimal)(last - first).TotalHours;
    }

    #endregion

    public interface Debug
    {
      void Log(string s);
    }
  }
}