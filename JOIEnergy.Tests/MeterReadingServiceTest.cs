namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using Domain;
  using Enums;
  using Services;
  using Xunit;

  public class MeterReadingServiceTest
  {
    private static readonly string _smartMeterID = "smart-meter-id";

    private readonly MeterReadingService _meterReadingService;

    public MeterReadingServiceTest()
    {
      _meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>(), null, null);

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
    public void GivenMeterIdThatExistsShouldReturnWeeklyUsage()
    {
      //arrange
      var smartMeterToPricePlanAccounts = new Dictionary<string, SupplierEnum> { { "smart-meter-id", SupplierEnum.TheGreenEco } };
      var pricePlans = new List<PricePlan>
      {
        new PricePlan { EnergySupplier = SupplierEnum.TheGreenEco, UnitRate = 2, PeakTimeMultiplier = noMultipliers() }
      };
      var readingService = new MeterReadingService
      (new Dictionary<string, List<ElectricityReading>>(),
        smartMeterToPricePlanAccounts,
        pricePlans);

      readingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddDays(-4), Reading = 35m },
          new ElectricityReading { Time = DateTime.Now.AddDays(-3), Reading = 30m }
        });

      const decimal expectedValue = 1560;

      //assert
      //TODO: change away from rolling week
      var actualCost = readingService.GetCostOfAWeekOfReadings(_smartMeterID, DateTime.Now.AddDays(-7));

      //act
      Assert.Equal(expectedValue, Math.Round(actualCost, 0));
    }

    [Fact]
    public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
    {
      _meterReadingService.StoreReadings
        (_smartMeterID, new List<ElectricityReading> { new ElectricityReading { Time = DateTime.Now, Reading = 25m } });

      var electricityReadings = _meterReadingService.GetReadings(_smartMeterID);

      Assert.Equal(3, electricityReadings.Count);
    }

    #region Support Methods

    private static List<PeakTimeMultiplier> noMultipliers()
    {
      return new List<PeakTimeMultiplier>();
    }

    #endregion
  }
}