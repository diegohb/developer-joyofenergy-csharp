namespace JOIEnergy.Services
{
  using System;
  using System.Collections.Generic;
  using Domain;

  public interface IMeterReadingService
  {
    decimal GetCostOfAWeekOfReadings(string smartMeterIDParam, DateTime startingDateParam);

    List<ElectricityReading> GetReadings(string smartMeterIdParam);

    void StoreReadings(string smartMeterIdParam, List<ElectricityReading> electricityReadingsParam);
  }
}