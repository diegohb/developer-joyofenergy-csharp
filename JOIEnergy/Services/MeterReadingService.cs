namespace JOIEnergy.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Domain;
  using Enums;

  public class MeterReadingService : IMeterReadingService
  {
    private readonly List<PricePlan> _pricePlans;
    private readonly Dictionary<string, SupplierEnum> _smartMeterToPricePlanAccounts;

    public MeterReadingService
    (Dictionary<string, List<ElectricityReading>> meterAssociatedReadingsParam, Dictionary<string, SupplierEnum> smartMeterToPricePlanAccountsParam,
      List<PricePlan> pricePlansParam)
    {
      _smartMeterToPricePlanAccounts = smartMeterToPricePlanAccountsParam;
      _pricePlans = pricePlansParam;

      MeterAssociatedReadings = meterAssociatedReadingsParam;
    }

    public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }


    public decimal GetCostOfAWeekOfReadings(string smartMeterIDParam, DateTime startingDateParam)
    {
      var readings = GetReadings(smartMeterIDParam);
      if (!readings.Any())
      {
        throw new ApplicationException("Unable to find smart reader by id.");
      }

      var suplierEnum = _smartMeterToPricePlanAccounts[smartMeterIDParam];
      var pricePlan = _pricePlans.Find(p => p.EnergySupplier.Equals(suplierEnum));
      return calculateCost(readings, pricePlan);
    }

    public List<ElectricityReading> GetReadings(string smartMeterIdParam)
    {
      if (MeterAssociatedReadings.ContainsKey(smartMeterIdParam))
      {
        return MeterAssociatedReadings[smartMeterIdParam];
      }

      return new List<ElectricityReading>();
    }

    public void StoreReadings(string smartMeterIdParam, List<ElectricityReading> electricityReadingsParam)
    {
      if (!MeterAssociatedReadings.ContainsKey(smartMeterIdParam))
      {
        MeterAssociatedReadings.Add(smartMeterIdParam, new List<ElectricityReading>());
      }

      electricityReadingsParam.ForEach
      (electricityReading =>
        MeterAssociatedReadings[smartMeterIdParam].Add(electricityReading));
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

    #endregion
  }
}