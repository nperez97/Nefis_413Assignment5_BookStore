using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; } //added set to configuration

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<BookStoreDbContext>(options =>
            {
                //go into configuration and set connectionstring key to BookStoreConnection, the name of the connection. Will have info for how we will connect
   
                options.UseSqlServer(Configuration.GetConnectionString("BookStoreConnection")); 
            });

            services.AddScoped<IBookStoreRepository, EFBookStoreRepository>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //add another map controller for filtering
                endpoints.MapControllerRoute("categorypage",
                    "Books/{category}/{page:int}",
                    new { Controller = "Home", action = "Index" });

                //if user only provides page for filtering
                endpoints.MapControllerRoute("page",
                    "{page:int}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapControllerRoute("category",
                    "Books/{category}",
                    new { Controller = "Home", action = "Index", page = 1 });

                //changes the way the url looks for page navigation
                endpoints.MapControllerRoute("pagination",
                    "P{page}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapDefaultControllerRoute();

            });

            SeedData.EnsurePopulated(app);
        }
    }
}
