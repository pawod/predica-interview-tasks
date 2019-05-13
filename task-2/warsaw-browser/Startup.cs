using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using task_2.Contracts;
using task_2.Infrastructure;
using task_2.Services;
using WarsawBrowser.Models;

namespace task_2
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.Configuration = configuration;
            this._logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataTableService, DataTableService>();

            services.Configure<AppSettings>(Configuration);


            services.AddAutoMapper(options => options.AddProfile<MappingProfile>());

            services.AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDbContext<MemoryDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase("MemoryDb");
                });


            //services.AddScoped<IAppConfig, AppConfig>(provider => Configuration.GetSection("appsettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            _logger.LogDebug($"Environment: {env.EnvironmentName}");

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
