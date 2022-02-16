namespace JOIEnergy.Generator
{
  using System;
  using System.Collections.Generic;
  using Domain;

  public class ElectricityReadingGenerator
  {
    public List<ElectricityReading> Generate(int numberParam)
    {
      var readings = new List<ElectricityReading>();
      var random = new Random();
      for (var i = 0; i < numberParam; i++)
      {
        var reading = (decimal)random.NextDouble();
        var electricityReading = new ElectricityReading { Reading = reading, Time = DateTime.Now.AddSeconds(-i * 10) };
        readings.Add(electricityReading);
      }

      readings.Sort((reading1, reading2) => reading1.Time.CompareTo(reading2.Time));
      return readings;
    }
  }
}