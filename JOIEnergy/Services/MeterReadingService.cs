namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Domain;

  public class MeterReadingService : IMeterReadingService
  {
    public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
    {
      MeterAssociatedReadings = meterAssociatedReadings;
    }

    public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }

    public List<ElectricityReading> GetReadings(string smartMeterId)
    {
      if (MeterAssociatedReadings.ContainsKey(smartMeterId))
      {
        return MeterAssociatedReadings[smartMeterId];
      }

      return new List<ElectricityReading>();
    }

    public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
    {
      if (!MeterAssociatedReadings.ContainsKey(smartMeterId))
      {
        MeterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
      }

      electricityReadings.ForEach
      (electricityReading =>
        MeterAssociatedReadings[smartMeterId].Add(electricityReading));
    }
  }
}