using chainsharp.common;
using chainsharp.logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace chainsharp.server
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });

    public static IWebHost InitializeStartup(string[] args)
    {
      var defaultlocalStorageLocation =
          $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}{Path.DirectorySeparatorChar}boxinterview{Path.DirectorySeparatorChar}{Constants.RootInstallFolder}";

      string logFolderLocation = $"{defaultlocalStorageLocation}{Path.DirectorySeparatorChar}{Constants.LogsFolderName}";
      var globalData = new GlobalData
      {
        Logger = new Logger(logFolderLocation, Constants.LogFileName),
        LocalStorageLocation = defaultlocalStorageLocation,
      };

      DataStore dataStore = new DataStore();

      var webHostBuilder = new WebHostBuilder()
          .CaptureStartupErrors(true)
          .UseKestrel(options =>
          {
            if (Constants.ListenerProtocolIsHttps)
            {
#pragma warning disable CS0162 // Unreachable code detected
              options.Listen(System.Net.IPAddress.Any, Convert.ToInt32(Constants.ListenerPort), listenOptions =>
#pragma warning restore CS0162 // Unreachable code detected
              {
                var httpsConnectionAdapterOptions = new Microsoft.AspNetCore.Server.Kestrel.Https.HttpsConnectionAdapterOptions
                {
                  SslProtocols = System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12,
                };

                //listenOptions.NoDelay = true;
                listenOptions.UseHttps(httpsConnectionAdapterOptions);
              });
            }
          })
          .ConfigureServices(
              services =>
              {
                services.AddSingleton(globalData);
                services.AddSingleton(dataStore);
              })
          .UseIISIntegration()
          .UseStartup<Startup>();

      if (!Constants.ListenerProtocolIsHttps)
      {
        webHostBuilder.UseUrls($"http://+:{Constants.ListenerPort}");
      }

      globalData.WebHost = webHostBuilder.Build();
      return globalData.WebHost;
    }
  }
}
