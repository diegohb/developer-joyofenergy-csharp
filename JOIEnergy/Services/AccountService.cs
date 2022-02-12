namespace JOIEnergy.Services
{
  using System.Collections.Generic;
  using Enums;

  public class AccountService : Dictionary<string, SupplierEnum>, IAccountService
  {
    private readonly Dictionary<string, SupplierEnum> _smartMeterToPricePlanAccounts;

    public AccountService(Dictionary<string, SupplierEnum> smartMeterToPricePlanAccountsParam)
    {
      _smartMeterToPricePlanAccounts = smartMeterToPricePlanAccountsParam;
    }

    public SupplierEnum GetPricePlanIdForSmartMeterId(string smartMeterIdParam)
    {
      if (!_smartMeterToPricePlanAccounts.ContainsKey(smartMeterIdParam))
      {
        return SupplierEnum.NullSupplier;
      }

      return _smartMeterToPricePlanAccounts[smartMeterIdParam];
    }
  }
}