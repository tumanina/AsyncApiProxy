using System.Collections.Generic;
using MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.Api.Configuration;

namespace AsyncApiProxy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<ISubscriptionFactory, SubscriptionFactory>();
            services.AddSingleton<ISenderProcessor, SenderProcessor>();
            services.AddSingleton<IClientService, ClientService>();

            var senders = Configuration.GetSection("Senders").Get<IEnumerable<SenderConfiguration>>();

            if (senders != null)
            {
                foreach (var sender in senders)
                {
                    services.AddSingleton<ISender>(t => new Sender(new ConnectionFactory
                    {
                        HostName = sender.Server.Host,
                        UserName = sender.Server.UserName,
                        Password = sender.Server.Password
                    },
                        sender.Type,
                        sender.QueueName,
                        sender.ExchangeName));
                }
            }

            var serviceProvider = services.BuildServiceProvider();

            var callbackServerConfiguration = Configuration.GetSection("CallBackServer").Get<ServerConfiguration>();

            services.AddSingleton<IRequestManager>(t => new RequestManager(
                new ConnectionFactory
                {
                    HostName = callbackServerConfiguration.Host,
                    UserName = callbackServerConfiguration.UserName,
                    Password = callbackServerConfiguration.Password
                },
                serviceProvider.GetService<ISenderProcessor>(),
                serviceProvider.GetService<ISubscriptionFactory>(),
                callbackServerConfiguration.TimeToWait
            ));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
