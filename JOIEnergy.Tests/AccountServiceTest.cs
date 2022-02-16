namespace JOIEnergy.Tests
{
  using System.Collections.Generic;
  using Enums;
  using Services;
  using Xunit;

  public class AccountServiceTest
  {
    private readonly AccountService _accountService;

    public AccountServiceTest()
    {
      var smartMeterToPricePlanAccounts = new Dictionary<string, SupplierEnum>();
      smartMeterToPricePlanAccounts.Add(smartMeterID, pricePlanID);

      _accountService = new AccountService(smartMeterToPricePlanAccounts);
    }

    private const SupplierEnum pricePlanID = SupplierEnum.PowerForEveryone;
    private const string smartMeterID = "smart-meter-id";

    [Fact]
    public void GivenAnUnknownSmartMeterIdReturnsANullSupplier()
    {
      var result = _accountService.GetPricePlanIdForSmartMeterId("bob-carolgees");
      Assert.Equal(SupplierEnum.NullSupplier, result);
    }

    [Fact]
    public void GivenTheSmartMeterIdReturnsThePricePlanId()
    {
      var result = _accountService.GetPricePlanIdForSmartMeterId("smart-meter-id");
      Assert.Equal(SupplierEnum.PowerForEveryone, result);
    }
  }
}