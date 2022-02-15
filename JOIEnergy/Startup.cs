namespace JOIEnergy
{
  using System.Collections.Generic;
  using System.Linq;
  using Domain;
  using Enums;
  using Generator;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Services;

  public class Startup
  {
    public Startup(IConfiguration configurationParam)
    {
      Configuration = configurationParam;
    }

    public IConfiguration Configuration { get; }

    public Dictionary<string, SupplierEnum> SmartMeterToPricePlanAccounts
    {
      get
      {
        var smartMeterToPricePlanAccounts = new Dictionary<string, SupplierEnum>();
        smartMeterToPricePlanAccounts.Add("smart-meter-0", SupplierEnum.DrEvilsDarkEnergy);
        smartMeterToPricePlanAccounts.Add("smart-meter-1", SupplierEnum.TheGreenEco);
        smartMeterToPricePlanAccounts.Add("smart-meter-2", SupplierEnum.DrEvilsDarkEnergy);
        smartMeterToPricePlanAccounts.Add("smart-meter-3", SupplierEnum.PowerForEveryone);
        smartMeterToPricePlanAccounts.Add("smart-meter-4", SupplierEnum.TheGreenEco);
        return smartMeterToPricePlanAccounts;
      }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder appParam, IHostingEnvironment envParam)
    {
      if (envParam.IsDevelopment())
      {
        appParam.UseDeveloperExceptionPage();
      }

      appParam.UseMvc();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection servicesParam)
    {
      var readings = generateMeterElectricityReadings();

      var pricePlans = new List<PricePlan>
      {
        new PricePlan { EnergySupplier = SupplierEnum.DrEvilsDarkEnergy, UnitRate = 10m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
        new PricePlan { EnergySupplier = SupplierEnum.TheGreenEco, UnitRate = 2m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
        new PricePlan { EnergySupplier = SupplierEnum.PowerForEveryone, UnitRate = 1m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() }
      };

      servicesParam.AddMvc();
      servicesParam.AddTransient<IAccountService, AccountService>();
      servicesParam.AddTransient<IMeterReadingService, MeterReadingService>();
      servicesParam.AddTransient<IPricePlanService, PricePlanService>();
      servicesParam.AddSingleton(arg => readings);
      servicesParam.AddSingleton(arg => pricePlans);
      servicesParam.AddSingleton(arg => SmartMeterToPricePlanAccounts);
    }

    #region Support Methods

    private Dictionary<string, List<ElectricityReading>> generateMeterElectricityReadings()
    {
      var readings = new Dictionary<string, List<ElectricityReading>>();
      var generator = new ElectricityReadingGenerator();
      var smartMeterIds = SmartMeterToPricePlanAccounts.Select(mtpp => mtpp.Key);

      foreach (var smartMeterId in smartMeterIds)
      {
        readings.Add(smartMeterId, generator.Generate(20));
      }

      return readings;
    }

    #endregion
  }
}