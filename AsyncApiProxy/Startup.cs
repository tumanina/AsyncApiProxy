using System.Collections.Generic;
using MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.Api.Configuration;
using AsyncApiProxy.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using AsyncApiProxy.DAL.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System;
using System.Reflection;

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
            var connectionString = Configuration.GetConnectionString("TaskDBConnectionString");

            var dbOptions = new DbContextOptionsBuilder<TaskContext>();
            dbOptions.UseSqlServer(connectionString);
            services.AddSingleton<ITaskContextFactory>(t => new TaskContextFactory(dbOptions));
            services.AddSingleton<ITaskContext, TaskContext>();
            services.AddSingleton<ISubscriptionFactory, SubscriptionFactory>();
            services.AddSingleton<ISenderProcessor, SenderProcessor>();
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<ITaskRepository, TaskRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "AsyncApi", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc();

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AsyncApi V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{id}");
            });
        }
    }
}
