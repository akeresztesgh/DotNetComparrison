using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSeedScript
{
    internal class Configure
    {
        public static IConfigurationRoot ConfigurationRoot { get; private set; }
        public static DbContextOptions<SchoolContext> DbOptions { get; private set; }

        public static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);                            
            
            ConfigurationRoot = builder.Build();
            var connString = ConfigurationRoot.GetConnectionString("SchoolContext");
            DbOptions = new DbContextOptionsBuilder<SchoolContext>()
                                .UseSqlServer(connString)
                                .Options;
        }


    }
}
