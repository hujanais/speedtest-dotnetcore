using Microsoft.Extensions.Configuration;
using SpeedTest.Net;
using SpeedTest.Net.Enums;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace speedtest_dotnetcore
{
    class Program
    {
        private static int UPDATERATE_MS = 60 * 60 * 1000;   // 60 mins default.  This will be overridden by the appsettings.json
        private static Timer timer = null;
        private static Mongoose mongoose = null;
        private static string pythonFullPath = "";
        private static string pythonCmd = "";

        static void Main(string[] args)
        {
            // read the appsettings.json
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            string mongodbUrl = configuration["MONGODB_URL"];
            string dbName = configuration["DB_NAME"];
            string collectionName = configuration["COLLECTION_NAME"];
            pythonFullPath = configuration["PYTHON_FULLPATH"];
            pythonCmd = configuration["PYTHON_CMD"];

            if (int.TryParse(configuration["REFRESH_RATE_MS"], out UPDATERATE_MS) == false)
            {
                throw new Exception("REFRESH_RATE_MS parsing failed.");
            }

            if (string.IsNullOrEmpty(mongodbUrl)) {
                throw new Exception("MONGODB_URL not found");
            }
            if (string.IsNullOrEmpty(dbName)) {
                throw new Exception("DB_NAME not found");
            }
            if (string.IsNullOrEmpty(collectionName)) {
                throw new Exception("COLLECTION_NAME not found");
            }
            if (string.IsNullOrEmpty(pythonFullPath))
            {
                throw new Exception("PYTHON_FULLPATH not found");
            }
            if (string.IsNullOrEmpty(pythonCmd))
            {
                throw new Exception("PYTHON_FULLPATH not found");
            }

            mongoose = new Mongoose(mongodbUrl, dbName, collectionName);
            createTimer();

            Console.WriteLine("Press any key to stop");
            Console.Read();  // This works with Linux.

        }

        static void createTimer()
        {
            // fire off the first run immediately.  not the best solution but good enough for this.
            Timer_Elapsed(null, null);

            timer = new Timer(UPDATERATE_MS);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var cmdRunner = new RunCmd();
            var result = cmdRunner.Run(pythonFullPath, pythonCmd, null);
            var localTime = DateTime.Now;
            if (result.IsSuccess)
            {
                mongoose.AddData(
                    result.Data.TimeStamp,
                    result.Data.ping,
                    result.Data.download / 1E6,
                    result.Data.upload / 1E6,
                    result.Data.ErrorMessage);
                Console.WriteLine(result.Data);
            }
            else
            {
                mongoose.AddData(
                   localTime,
                   -1,
                   -1,
                   -1,
                   result.Data.ErrorMessage);
                Console.WriteLine($"Error exited with ${result.Data}");
            }
        }
    }
}