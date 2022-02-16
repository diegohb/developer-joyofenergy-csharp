namespace JOIEnergy.Services
{
  using Enums;

  public interface IAccountService
  {
    SupplierEnum GetPricePlanIdForSmartMeterId(string smartMeterIdParam);
  }
}