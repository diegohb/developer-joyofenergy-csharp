namespace JOIEnergy.Tests
{
  using System.Collections.Generic;
  using Enums;
  using Services;
  using Xunit;

  public class AccountServiceTest
  {
    public AccountServiceTest()
    {
      var smartMeterToPricePlanAccounts = new Dictionary<string, Supplier>();
      smartMeterToPricePlanAccounts.Add(SMART_METER_ID, PRICE_PLAN_ID);

      accountService = new AccountService(smartMeterToPricePlanAccounts);
    }

    private const Supplier PRICE_PLAN_ID = Supplier.PowerForEveryone;
    private const string SMART_METER_ID = "smart-meter-id";

    private readonly AccountService accountService;

    [Fact]
    public void GivenAnUnknownSmartMeterIdReturnsANullSupplier()
    {
      var result = accountService.GetPricePlanIdForSmartMeterId("bob-carolgees");
      Assert.Equal(Supplier.NullSupplier, result);
    }

    [Fact]
    public void GivenTheSmartMeterIdReturnsThePricePlanId()
    {
      var result = accountService.GetPricePlanIdForSmartMeterId("smart-meter-id");
      Assert.Equal(Supplier.PowerForEveryone, result);
    }
  }
}