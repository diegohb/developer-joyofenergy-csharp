namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using Domain;
  using Services;
  using Xunit;

  public class MeterReadingServiceTest
  {
    private static readonly string _smartMeterID = "smart-meter-id";

    private readonly MeterReadingService _meterReadingService;

    public MeterReadingServiceTest()
    {
      _meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

      _meterReadingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddDays(-4).AddMinutes(-30), Reading = 35m },
          new ElectricityReading { Time = DateTime.Now.AddDays(-3).AddMinutes(-15), Reading = 30m }
        });
    }

    [Fact]
    public void GivenMeterIdThatDoesNotExistShouldReturnNull()
    {
      Assert.Empty(_meterReadingService.GetReadings("unknown-id"));
    }

    [Fact]
    public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
    {
      _meterReadingService.StoreReadings
        (_smartMeterID, new List<ElectricityReading> { new ElectricityReading { Time = DateTime.Now, Reading = 25m } });

      var electricityReadings = _meterReadingService.GetReadings(_smartMeterID);

      Assert.Equal(3, electricityReadings.Count);
    }
  }
}