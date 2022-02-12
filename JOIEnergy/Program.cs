namespace JOIEnergy
{
  using Microsoft.AspNetCore;
  using Microsoft.AspNetCore.Hosting;

  public class Program
  {
    public static IWebHost BuildWebHost(string[] argsParam)
    {
      return WebHost.CreateDefaultBuilder(argsParam)
        .UseStartup<Startup>()
        .Build();
    }

    public static void Main(string[] argsParam)
    {
      BuildWebHost(argsParam).Run();
    }
  }
}