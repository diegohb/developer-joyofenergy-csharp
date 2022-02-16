namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using Domain;
  using Enums;
  using Services;
  using Xunit;

  public class UsageCostTests
  {
    private static readonly string _smartMeterID = "smart-meter-id";

    [Fact]
    public void GivenMeterIdThatExistsShouldReturnWeeklyUsage()
    {
      //arrange
      var smartMeterToPricePlanAccounts = new Dictionary<string, SupplierEnum> { { "smart-meter-id", SupplierEnum.TheGreenEco } };
      var pricePlans = new List<PricePlan>
      {
        new PricePlan { EnergySupplier = SupplierEnum.TheGreenEco, UnitRate = 2, PeakTimeMultiplier = noMultipliers() }
      };
      var readingService = new UsageService
      (new Dictionary<string, List<ElectricityReading>>
      {
        {
          "smart-meter-id",
          new List<ElectricityReading>
          {
            new ElectricityReading { Time = DateTime.Now.AddDays(-4), Reading = 35m },
            new ElectricityReading { Time = DateTime.Now.AddDays(-3), Reading = 30m }
          }
        }
      }, smartMeterToPricePlanAccounts, pricePlans);

      const decimal expectedValue = 1560;

      //assert
      //TODO: change away from rolling week
      var actualCost = readingService.GetCostOfAWeekOfReadings(_smartMeterID, DateTime.Now.AddDays(-7));

      //act
      Assert.Equal(expectedValue, Math.Round(actualCost, 0));
    }

    #region Support Methods

    private static List<PeakTimeMultiplier> noMultipliers()
    {
      return new List<PeakTimeMultiplier>();
    }

    #endregion
  }
}