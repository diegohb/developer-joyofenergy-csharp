namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Domain;

  public interface IMeterReadingService
  {
    List<ElectricityReading> GetReadings(string smartMeterIdParam);

    void StoreReadings(string smartMeterIdParam, List<ElectricityReading> electricityReadingsParam);
  }
}