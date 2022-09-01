namespace Atm.RestApi
{
    using Atm.Application.Interfaces;
    using Atm.Application.Services;
    using Atm.Infrastructure.Persistence;
    using Atm.RestApi.Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

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
            services.AddControllers(options => options.Filters.Add(new StatusCodeRelatedExceptionFilter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atm.RestApi", Version = "v1" });
            });

            services.AddDbContext<AtmDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AtmDbConnection"));
            });

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IAtmUnitOfWork, AtmUnitOfWork>();
            services.AddTransient<IAtmService, AtmService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atm.RestApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AtmDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}