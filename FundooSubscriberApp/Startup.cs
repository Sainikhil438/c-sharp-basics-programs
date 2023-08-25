using FundooSubscriberApp.Interface;
using FundooSubscriberApp.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooSubscriberApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<ConnectionFactory>(_ => new ConnectionFactory
            {
                HostName = Configuration["RabbitMQSettings:HostName"],
                UserName = Configuration["RabbitMQSettings:UserName"],
                Password = Configuration["RabbitMQSettings:Password"]
            });

            services.AddScoped<IRabbitMQSubscriber>(provider =>
            {
                var factory = provider.GetRequiredService<ConnectionFactory>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var busControl = provider.GetRequiredService<IBusControl>();
                return new RabbitMQSubscriber(factory, configuration, busControl);
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegistrationEmailSubscriber>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQSettings:HostName"], h =>
                    {
                        h.Username(Configuration["RabbitMQSettings:UserName"]);
                        h.Password(Configuration["RabbitMQSettings:Password"]);
                    });
                });
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
