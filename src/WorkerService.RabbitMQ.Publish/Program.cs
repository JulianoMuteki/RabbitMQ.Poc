using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerService.RabbitMQ.Publish
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
                    var rabbitConfig = configuration.GetSection("rabbit");
                    services.Configure<RabbitOptions>(rabbitConfig);

                    services.AddTransient<ObjectPoolProvider, DefaultObjectPoolProvider>();
                    services.AddTransient<IPooledObjectPolicy<IModel>, RabbitFactory>();
                    services.AddTransient<IMessageBroker, MessageBroker>();

                    services.AddHostedService<Worker>();
                });
    }
}
