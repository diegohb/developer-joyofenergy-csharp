namespace JOIEnergy.Tests
{
  using System;
  using System.Collections.Generic;
  using Domain;
  using Enums;
  using Xunit;

  public class PricePlanTest
  {
    private readonly PricePlan _pricePlan;

    public PricePlanTest()
    {
      _pricePlan = new PricePlan
      {
        EnergySupplier = SupplierEnum.TheGreenEco,
        UnitRate = 20m,
        PeakTimeMultiplier = new List<PeakTimeMultiplier>
        {
          new PeakTimeMultiplier { DayOfWeek = DayOfWeek.Saturday, Multiplier = 2m },
          new PeakTimeMultiplier { DayOfWeek = DayOfWeek.Sunday, Multiplier = 10m }
        }
      };
    }

    [Fact]
    public void TestGetBasePrice()
    {
      Assert.Equal(20m, _pricePlan.GetPrice(new DateTime(2018, 1, 2)));
    }

    [Fact]
    public void TestGetEnergySupplier()
    {
      Assert.Equal(SupplierEnum.TheGreenEco, _pricePlan.EnergySupplier);
    }

    [Fact]
    public void TestGetPeakTimePrice()
    {
      Assert.Equal(40m, _pricePlan.GetPrice(new DateTime(2018, 1, 6)));
    }
  }
}