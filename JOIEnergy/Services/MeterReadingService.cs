namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Domain;

  public class MeterReadingService : IMeterReadingService
  {
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