using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpRequestProcessing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configBuilder =>
                {
                    var config = configBuilder.Build();
                    var storagePath = config.GetValue<string>("FileSystemMessageBus:StorageFolderPath");
                    if (!string.IsNullOrWhiteSpace(storagePath))
                    {
                        var filePath = Path.Combine(storagePath, "messageBusSettings.json");
                        configBuilder.AddJsonFile(filePath);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
