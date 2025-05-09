using HotelSystem.HotelSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace HotelSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Konfigurera appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .BuildServiceProvider();

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Seeda databasen vid första körning
                    context.Database.EnsureCreated();
                    var hotelApp = new HotelApp(context);
                    var menu = new Menu(hotelApp);
                    menu.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
            }
        }
    }
}
