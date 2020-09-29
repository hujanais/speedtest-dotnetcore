using System;
using System.Collections.Generic;
using System.Text;

namespace speedtest_dotnetcore
{
    // Server ID,Sponsor,Server Name,Timestamp,Distance,Ping,Download,Upload,Share,IP Address
    public class SpeedTestData
    {
        public float ping { get; set; }
        public double download { get; set; }
        public double upload { get; set; }

        // TimeStamp and ErrorMessage is not part of the JSON.
        public DateTime TimeStamp { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return $"{this.TimeStamp}, {this.ping}, {this.download}, {this.upload}";
        }
    }
}
