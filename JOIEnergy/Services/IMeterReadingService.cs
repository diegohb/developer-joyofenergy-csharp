namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Domain;

  public interface IMeterReadingService
  {
    List<ElectricityReading> GetReadings(string smartMeterId);

    void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
  }
}