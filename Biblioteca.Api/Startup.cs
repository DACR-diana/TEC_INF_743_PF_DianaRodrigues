using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Core;
using Biblioteca.Core.Repositories;
using Biblioteca.Core.Repositories.Books;
using Biblioteca.Core.Repositories.Users;
using Biblioteca.Core.Services;
using Biblioteca.Core.Services.Books;
using Biblioteca.Core.Services.Checkouts;
using Biblioteca.Core.Services.Users;
using Biblioteca.Data;
using Biblioteca.Data.Repositories;
using Biblioteca.Data.Repositories.Books;
using Biblioteca.Data.Repositories.Users;
using Biblioteca.Services;
using Biblioteca.Services.Books;
using Biblioteca.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Api
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("Biblioteca.Data")));
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<ICategoryService, CategoryService>();
            //services.AddTransient<IEmployeeService, EmployeeService>();
            //services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ICheckoutService, CheckoutService>();
            ////services.AddTransient<ICountryService, CountryService>();
            ////services.AddTransient<IPaymentService, PaymentService>();
            //services.AddTransient<ITicketService, TicketService>();

            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api for Biblioteca Project (Tec.Inf.)");
            });

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
