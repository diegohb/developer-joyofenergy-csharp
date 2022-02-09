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
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public Dictionary<string, Supplier> SmartMeterToPricePlanAccounts
    {
      get
      {
        var smartMeterToPricePlanAccounts = new Dictionary<string, Supplier>();
        smartMeterToPricePlanAccounts.Add("smart-meter-0", Supplier.DrEvilsDarkEnergy);
        smartMeterToPricePlanAccounts.Add("smart-meter-1", Supplier.TheGreenEco);
        smartMeterToPricePlanAccounts.Add("smart-meter-2", Supplier.DrEvilsDarkEnergy);
        smartMeterToPricePlanAccounts.Add("smart-meter-3", Supplier.PowerForEveryone);
        smartMeterToPricePlanAccounts.Add("smart-meter-4", Supplier.TheGreenEco);
        return smartMeterToPricePlanAccounts;
      }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var readings =
        GenerateMeterElectricityReadings();

      var pricePlans = new List<PricePlan>
      {
        new PricePlan { EnergySupplier = Supplier.DrEvilsDarkEnergy, UnitRate = 10m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
        new PricePlan { EnergySupplier = Supplier.TheGreenEco, UnitRate = 2m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() },
        new PricePlan { EnergySupplier = Supplier.PowerForEveryone, UnitRate = 1m, PeakTimeMultiplier = new List<PeakTimeMultiplier>() }
      };

      services.AddMvc();
      services.AddTransient<IAccountService, AccountService>();
      services.AddTransient<IMeterReadingService, MeterReadingService>();
      services.AddTransient<IPricePlanService, PricePlanService>();
      services.AddSingleton(arg => readings);
      services.AddSingleton(arg => pricePlans);
      services.AddSingleton(arg => SmartMeterToPricePlanAccounts);
    }

    #region Support Methods

    private Dictionary<string, List<ElectricityReading>> GenerateMeterElectricityReadings()
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