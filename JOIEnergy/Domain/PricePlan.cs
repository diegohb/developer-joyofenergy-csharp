namespace JOIEnergy.Domain
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Enums;

  public class PricePlan
  {
    public Supplier EnergySupplier { get; set; }
    public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set; }
    public decimal UnitRate { get; set; }

    public decimal GetPrice(DateTime datetime)
    {
      var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

      if (multiplier?.Multiplier != null)
      {
        return multiplier.Multiplier * UnitRate;
      }

      return UnitRate;
    }
  }

  public class PeakTimeMultiplier
  {
    public DayOfWeek DayOfWeek { get; set; }
    public decimal Multiplier { get; set; }
  }
}