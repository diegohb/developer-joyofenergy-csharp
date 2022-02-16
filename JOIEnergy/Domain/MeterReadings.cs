namespace JOIEnergy.Domain
{
  using System.Collections.Generic;

  public class MeterReadings
  {
    public List<ElectricityReading> ElectricityReadings { get; set; }
    public string SmartMeterId { get; set; }
  }
}