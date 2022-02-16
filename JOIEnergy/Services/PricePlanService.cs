namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using System.Linq;
  using Domain;

  public class PricePlanService : IPricePlanService
  {
    private readonly IMeterReadingService _meterReadingService;
    private readonly List<PricePlan> _pricePlans;

    public PricePlanService(List<PricePlan> pricePlanParam, IMeterReadingService meterReadingServiceParam)
    {
      _pricePlans = pricePlanParam;
      _meterReadingService = meterReadingServiceParam;
    }

    public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterIdParam)
    {
      var electricityReadings = _meterReadingService.GetReadings(smartMeterIdParam);

      if (!electricityReadings.Any())
      {
        return new Dictionary<string, decimal>();
      }

      return _pricePlans.ToDictionary
      (plan => plan.EnergySupplier.ToString(),
        plan => calculateCost(electricityReadings, plan));
    }

    #region Support Methods

    private decimal calculateAverageReading(List<ElectricityReading> electricityReadingsParam)
    {
      var newSummedReadings = electricityReadingsParam.Select(readings => readings.Reading)
        .Aggregate((reading, accumulator) => reading + accumulator);

      return newSummedReadings / electricityReadingsParam.Count();
    }

    private decimal calculateCost(List<ElectricityReading> electricityReadingsParam, PricePlan pricePlanParam)
    {
      var average = calculateAverageReading(electricityReadingsParam);
      var timeElapsed = calculateTimeElapsed(electricityReadingsParam);
      var averagedCost = average / timeElapsed;
      return averagedCost * pricePlanParam.UnitRate;
    }

    private decimal calculateTimeElapsed(List<ElectricityReading> electricityReadingsParam)
    {
      var first = electricityReadingsParam.Min(reading => reading.Time);
      var last = electricityReadingsParam.Max(reading => reading.Time);

      return (decimal)(last - first).TotalHours;
    }

    #endregion

    public interface IDebug
    {
      void Log(string sParam);
    }
  }
}