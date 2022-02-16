namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Domain;
  using Enums;
  using Newtonsoft.Json.Linq;
  using Services;
  using Xunit;

  public class PricePlanComparisonTest
  {
    private static readonly string _smartMeterID = "smart-meter-id";
    private readonly PricePlanComparatorController _controller;
    private readonly MeterReadingService _meterReadingService;

    private readonly Dictionary<string, SupplierEnum>
      _smartMeterToPricePlanAccounts = new Dictionary<string, SupplierEnum>();

    public PricePlanComparisonTest()
    {
      var readings = new Dictionary<string, List<ElectricityReading>>();
      _meterReadingService = new MeterReadingService(readings, null, null);
      var pricePlans = new List<PricePlan>
      {
        new PricePlan { EnergySupplier = SupplierEnum.DrEvilsDarkEnergy, UnitRate = 10, PeakTimeMultiplier = noMultipliers() },
        new PricePlan { EnergySupplier = SupplierEnum.TheGreenEco, UnitRate = 2, PeakTimeMultiplier = noMultipliers() },
        new PricePlan { EnergySupplier = SupplierEnum.PowerForEveryone, UnitRate = 1, PeakTimeMultiplier = noMultipliers() }
      };
      var pricePlanService = new PricePlanService(pricePlans, _meterReadingService);
      var accountService = new AccountService(_smartMeterToPricePlanAccounts);
      _controller = new PricePlanComparatorController(pricePlanService, accountService);
    }

    [Fact]
    public void GivenNoMatchingMeterIdShouldReturnNotFound()
    {
      Assert.Equal(404, _controller.CalculatedCostForEachPricePlan("not-found").StatusCode);
    }

    [Fact]
    public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
    {
      var electricityReading = new ElectricityReading { Time = DateTime.Now.AddHours(-1), Reading = 15.0m };
      var otherReading = new ElectricityReading { Time = DateTime.Now, Reading = 5.0m };
      _meterReadingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading> { electricityReading, otherReading });

      var result = _controller.CalculatedCostForEachPricePlan(_smartMeterID).Value;

      var actualCosts = ((JObject)result).ToObject<Dictionary<string, decimal>>();
      Assert.Equal(3, actualCosts.Count);
      Assert.Equal(100m, actualCosts["" + SupplierEnum.DrEvilsDarkEnergy], 3);
      Assert.Equal(20m, actualCosts["" + SupplierEnum.TheGreenEco], 3);
      Assert.Equal(10m, actualCosts["" + SupplierEnum.PowerForEveryone], 3);
    }

    [Fact]
    public void ShouldRecommendCheapestPricePlansMoreThanLimitAvailableForMeterUsage()
    {
      _meterReadingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
          new ElectricityReading { Time = DateTime.Now, Reading = 3m }
        });

      var result = _controller.RecommendCheapestPricePlans(_smartMeterID, 5).Value;
      var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

      Assert.Equal(3, recommendations.Count);
    }

    [Fact]
    public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
    {
      _meterReadingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
          new ElectricityReading { Time = DateTime.Now, Reading = 3m }
        });

      var result = _controller.RecommendCheapestPricePlans(_smartMeterID).Value;
      var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

      Assert.Equal("" + SupplierEnum.PowerForEveryone, recommendations[0].Key);
      Assert.Equal("" + SupplierEnum.TheGreenEco, recommendations[1].Key);
      Assert.Equal("" + SupplierEnum.DrEvilsDarkEnergy, recommendations[2].Key);
      Assert.Equal(38m, recommendations[0].Value, 3);
      Assert.Equal(76m, recommendations[1].Value, 3);
      Assert.Equal(380m, recommendations[2].Value, 3);
      Assert.Equal(3, recommendations.Count);
    }

    [Fact]
    public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage()
    {
      _meterReadingService.StoreReadings
      (_smartMeterID,
        new List<ElectricityReading>
        {
          new ElectricityReading { Time = DateTime.Now.AddMinutes(-45), Reading = 5m },
          new ElectricityReading { Time = DateTime.Now, Reading = 20m }
        });

      var result = _controller.RecommendCheapestPricePlans(_smartMeterID, 2).Value;
      var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

      Assert.Equal("" + SupplierEnum.PowerForEveryone, recommendations[0].Key);
      Assert.Equal("" + SupplierEnum.TheGreenEco, recommendations[1].Key);
      Assert.Equal(16.667m, recommendations[0].Value, 3);
      Assert.Equal(33.333m, recommendations[1].Value, 3);
      Assert.Equal(2, recommendations.Count);
    }

    #region Support Methods

    private static List<PeakTimeMultiplier> noMultipliers()
    {
      return new List<PeakTimeMultiplier>();
    }

    #endregion
  }
}