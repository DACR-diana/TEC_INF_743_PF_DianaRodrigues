using AutoMapper;
using Biblioteca.Api.Extensions;
using Biblioteca.Core;
using Biblioteca.Core.Services;
using Biblioteca.Core.Services.Books;
using Biblioteca.Core.Services.Checkouts;
using Biblioteca.Core.Services.Users;
using Biblioteca.Data;
using Biblioteca.Services;
using Biblioteca.Services.Books;
using Biblioteca.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

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
            // LOCALIZATION
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("pt-PT"),
                        new CultureInfo("es-ES")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "pt-PT", uiCulture: "pt-PT");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new[] { new Extensions.RouteDataRequestCultureProvider { IndexOfCulture = 1, IndexofUICulture = 1 } };
                });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });
            services.AddControllers();


            // DBCONTEXT
            services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("Biblioteca.Data")));

            // SERVICES
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookAuthorService, BookAuthorService>();
            services.AddTransient<IBookCategoryService, BookCategoryService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ICheckoutService, CheckoutService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddScoped<ITicketService, TicketService>();

            // DOCUMENTATION
            services.AddSwaggerGen();

            // AUTO MAPPER
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

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{culture:culture}/api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
