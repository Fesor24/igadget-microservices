using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderSaga.Entities;

namespace OrderSaga
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<OrderStateDbContext>(opt =>
                        opt.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                    
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        
                        x.SetEntityFrameworkSagaRepositoryProvider(opts =>
                        {
                            opts.ExistingDbContext<OrderStateDbContext>();
                            opts.UsePostgres();
                        });

                        x.AddSagaStateMachine<OrderStateMachine, OrderStateData>()
                            .EntityFrameworkRepository(r =>
                            {
                                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                                r.ExistingDbContext<OrderStateDbContext>();
                                r.UsePostgres();
                            });

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        //x.AddSagaStateMachines(entryAssembly);
                        //x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);
                        
                        x.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });
                });
    }
}
