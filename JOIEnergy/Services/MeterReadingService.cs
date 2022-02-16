namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Domain;
  using Enums;

  public class MeterReadingService : IMeterReadingService
  {
    private readonly List<PricePlan> _pricePlans;
    private readonly Dictionary<string, SupplierEnum> _smartMeterToPricePlanAccounts;
    private readonly UsageService _usageService;

    public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadingsParam)
    {
      MeterAssociatedReadings = meterAssociatedReadingsParam;
    }

    public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }


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
  }
}