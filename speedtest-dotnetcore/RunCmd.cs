using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace speedtest_dotnetcore
{
    public class RunCmd
    {
        public ReturnValue Run(string pythonLocationFullPath, string cmd, string args)
        {
            ReturnValue result = null;
            ProcessStartInfo start = new ProcessStartInfo();
            // Server ID,Sponsor,Server Name,Timestamp,Distance,Ping,Download,Upload,Share,IP Address
            start.FileName = pythonLocationFullPath;
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            string jsonString = null;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    jsonString = reader.ReadToEnd();
                }
            }

            try
            {
                SpeedTestData speedTestData = JsonSerializer.Deserialize<SpeedTestData>(jsonString);
                speedTestData.TimeStamp = DateTime.UtcNow;
                result = new ReturnValue(true, speedTestData);
            } catch (Exception ex)
            {
                result = new ReturnValue(false, new SpeedTestData()
                {
                    TimeStamp = DateTime.UtcNow,
                    ErrorMessage = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message
                });
            }

            return result;
        }
    }
}
 