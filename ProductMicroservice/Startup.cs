using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ProductMicroservice.DBContexts;
using ProductMicroservice.Repository;
using System.Collections.Generic;
using ProductMicroservice.Models;


namespace ProductMicroservice
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //This is for local db connection string
             services.AddDbContext<ProductContext>(o => o.UseSqlServer(Configuration.GetConnectionString("ProductsDBConString")));
            //This is Azure Database connection string
           // services.AddDbContext<ProductContext>(o => o.UseSqlServer(Configuration.GetConnectionString("AZURE_SQL_CONNECTION")));
           
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product Api",
                    Description = "A simple API to create a product",
                    Contact = new OpenApiContact
                    {
                        Name = "Mehrdad Zandi",
                        Email = "mehrdad.zandi@softsolutionsahand.com",
                        Url = new Uri("http://www.softsolutionsahand.com/")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddControllers();

        }

        
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Allow Swagger UI loads in production Environment too (as Azure App Service)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
       
    }
}
