using SpeedTest.Net;
using SpeedTest.Net.Enums;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace speedtest_dotnetcore
{
    class Program
    {
        private static Timer timer = null;
        private static Mongoose mongoose = null;

        static void Main(string[] args)
        {
            mongoose = new Mongoose();
            createTimer();

            Console.WriteLine("Press any key to stop");
            Console.ReadKey();

        }

        static void createTimer()
        {
            // Create a timer with a two second interval.
            timer = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += Timer_Elapsed; ;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var cmdRunner = new RunCmd();
            var result = cmdRunner.Run(@"speedtest-cli\hello.py --csv", null);
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
                   result.Data.TimeStamp,
                   -1,
                   -1,
                   -1,
                   result.Data.ErrorMessage);
                Console.WriteLine($"Error exited with ${result.Data}");
            }
        }
    }
}