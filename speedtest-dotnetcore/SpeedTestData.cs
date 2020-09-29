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
        public DateTime TimeStamp { get; set; }

        // ErrorMessage is not part of the JSON.
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return $"{this.TimeStamp}, {this.ping}, {this.download}, {this.upload}";
        }
    }
}
