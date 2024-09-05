using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarPartsApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string frontend = config["Frontend"];
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), config["JsonFilePath"]);
            var carPartsService = new CarPartsService();
            await carPartsService.LoadDataAsync(jsonFilePath);

            if (frontend == "WinForms")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(carPartsService));
            }
            else if (frontend == "WebAPI")
            {
                CreateHostBuilder(args, carPartsService).Build().Run();
            }
            else
            {
                Console.WriteLine("Unknown frontend in config.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, CarPartsService carPartsService) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
                 .ConfigureServices(services =>
                 {
                     services.AddSingleton(carPartsService);
                 });
    }
}
