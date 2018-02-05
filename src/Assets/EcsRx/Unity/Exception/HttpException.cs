using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Unity.Exception
{
    public class HttpException : System.Exception
    {
        public int ErrorCode { get; set; }

        public HttpException(string message, int code) : base(message)
        {
            ErrorCode = code;
        }
    }
}
