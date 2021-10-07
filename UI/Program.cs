using StoreBL;
using DL;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Serilog;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../logs/logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information("Application Starting...");

            string connectionString = File.ReadAllText(@"../connectionString.txt");
            DbContextOptions<IIDBContext> options = new DbContextOptionsBuilder<IIDBContext>().UseSqlServer(connectionString).Options;
            IIDBContext context = new IIDBContext(options);
            Log.Information("First connection to DB successful");

            new MainMenu(new BL(new DBRepo(context))).Start();
            
            Log.Information("Application Closing...");
        }
    }
}
