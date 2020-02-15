using Vejledningsbooking.API.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Vejledningsbooking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            //TODO: In-Memory Db
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<VejledningsbookingContext>();

                context.Database.EnsureCreated();
            }
            // migrate the database.  Best practice = in Main, using service scope

            // Sql using
            //using (var scope = host.Services.CreateScope())
            //{
            //    try
            //    {
            //        var context = scope.ServiceProvider.GetService<VejledningsbookingContext>();
            //        // for demo purposes, delete the database & migrate on startup so 
            //        // we can start with a clean slate
            //        context.Database.EnsureDeleted();
            //        context.Database.Migrate();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while migrating the database.");
            //    }
            //}
            host.Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
