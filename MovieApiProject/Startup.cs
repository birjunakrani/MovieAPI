using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieApiProject.Controllers;
using MovieApiProject.Data;
using MovieApiProject.Services;

namespace MovieApiProject
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            var connectionString = _config["connectionStrings:MovieDbConnectionString"];
            services.AddDbContext<MovieDbContext>(c => c.UseSqlServer(connectionString));
            //OR
            //services.AddDbContext<MovieDbContext>(c => c.UseSqlServer(_config.GetConnectionString(MovieDbConnectionString));
           //services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<ICountryRepository,CountryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICriticRepository, CriticRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MovieDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            
            context.seedDataContext();
            app.UseMvc();
        }
    }
}
