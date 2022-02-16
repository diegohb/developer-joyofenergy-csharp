namespace JOIEnergy.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Domain;
  using Enums;

  public class UsageService
  {
    private readonly Dictionary<string, List<ElectricityReading>> _meterReadings;
    private readonly List<PricePlan> _pricePlans;
    private readonly Dictionary<string, SupplierEnum> _smartMeterToPricePlanAccounts;

    public UsageService
    (Dictionary<string, List<ElectricityReading>> meterAssociatedReadingsParam, Dictionary<string, SupplierEnum> smartMeterToPricePlanAccountsParam,
      List<PricePlan> pricePlansParam)
    {
      _meterReadings = meterAssociatedReadingsParam;
      _smartMeterToPricePlanAccounts = smartMeterToPricePlanAccountsParam;
      _pricePlans = pricePlansParam;
    }

    public decimal GetCostOfAWeekOfReadings(string smartMeterIDParam, DateTime startingDateParam)
    {
      var readings = getReadings(smartMeterIDParam);
      if (!readings.Any())
      {
        throw new ApplicationException("Unable to find smart reader by id.");
      }

      var suplierEnum = _smartMeterToPricePlanAccounts[smartMeterIDParam];
      var pricePlan = _pricePlans.Find(p => p.EnergySupplier.Equals(suplierEnum));
      return calculateCost(readings, pricePlan);
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
      var averagedCost = average * timeElapsed;
      return averagedCost * pricePlanParam.UnitRate;
    }

    private decimal calculateTimeElapsed(List<ElectricityReading> electricityReadingsParam)
    {
      var first = electricityReadingsParam.Min(reading => reading.Time);
      var last = electricityReadingsParam.Max(reading => reading.Time);

      return (decimal)(last - first).TotalHours;
    }

    private List<ElectricityReading> getReadings(string smartMeterIdParam)
    {
      if (_meterReadings.ContainsKey(smartMeterIdParam))
      {
        return _meterReadings[smartMeterIdParam];
      }

      return new List<ElectricityReading>();
    }

    #endregion
  }
}