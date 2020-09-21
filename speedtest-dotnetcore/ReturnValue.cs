using System;
using System.Collections.Generic;
using System.Text;

namespace speedtest_dotnetcore
{
    public class ReturnValue
    {
        public ReturnValue(bool isOk, SpeedTestData data)
        {
            this.IsSuccess = isOk;
            this.Data = data;
        }

        public bool IsSuccess { get; private set; }
        public SpeedTestData Data { get; private set; }
    }
}
