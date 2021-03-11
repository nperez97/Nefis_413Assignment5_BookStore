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
using Microsoft.AspNetCore.Http;

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
   
                options.UseSqlite(Configuration.GetConnectionString("BookStoreConnection")); 
            });

            services.AddScoped<IBookStoreRepository, EFBookStoreRepository>();

            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //add another map controller for filtering
                endpoints.MapControllerRoute("categorypage",
                    "Books/{category}/{pageNum:int}",
                    new { Controller = "Home", action = "Index" });

                //if user only provides page for filtering
                endpoints.MapControllerRoute("page",
                    "{pageNum:int}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapControllerRoute("category",
                    "Books/{category}",
                    new { Controller = "Home", action = "Index", pageNum = 1 });

                //changes the way the url looks for page navigation
                endpoints.MapControllerRoute("pagination",
                    "P{pageNum}",
                    new { Controller = "Home", action = "Index" });

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();

            });

            SeedData.EnsurePopulated(app);
        }
    }
}
