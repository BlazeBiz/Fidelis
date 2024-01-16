using MarauderServer.Data;
using MarauderServer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarauderServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy allowing any origin
            // TODO: Allow only specific origins
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            AddServices(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            // Get the datbase connection string
            string connectionString = builder.Configuration.GetConnectionString("Default");

            // Setup dependency injection so services and DAOs automatically get created when a controler fires
            // DAOs
            builder.Services.AddSingleton<CustomerAddressData, CustomerAddressData>(sp => new CustomerAddressData(connectionString));
            builder.Services.AddSingleton<CustomerData, CustomerData>(sp => new CustomerData(connectionString, sp.GetRequiredService<CustomerAddressData>()));
            builder.Services.AddSingleton<SalesOrderData, SalesOrderData>(sp => new SalesOrderData(connectionString, sp.GetRequiredService<CustomerData>()));
            //Services

            builder.Services.AddSingleton<CustomerService, CustomerService>(sp => new CustomerService(sp.GetRequiredService<CustomerData>()));
            builder.Services.AddSingleton<SalesOrderService, SalesOrderService>(sp => new SalesOrderService(sp.GetRequiredService<SalesOrderData>()));
        }
    }
}