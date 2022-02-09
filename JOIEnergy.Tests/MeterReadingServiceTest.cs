namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using Domain;
  using Services;
  using Xunit;

  public class MeterReadingServiceTest
  {
    public MeterReadingServiceTest()
    {
      meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

      meterReadingService.StoreReadings
      (SMART_METER_ID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
          new ElectricityReading { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
        });
    }

    private static readonly string SMART_METER_ID = "smart-meter-id";

    private readonly MeterReadingService meterReadingService;

    [Fact]
    public void GivenMeterIdThatDoesNotExistShouldReturnNull()
    {
      Assert.Empty(meterReadingService.GetReadings("unknown-id"));
    }

    [Fact]
    public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
    {
      meterReadingService.StoreReadings
        (SMART_METER_ID, new List<ElectricityReading> { new ElectricityReading { Time = DateTime.Now, Reading = 25m } });

      var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

      Assert.Equal(3, electricityReadings.Count);
    }
  }
}