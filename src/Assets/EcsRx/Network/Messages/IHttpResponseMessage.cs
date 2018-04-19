using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IHttpResponseMessage : IResponseMessage
    {
        bool IsOK { get; }
        string ErrorMessage { get; }
        int ErrorCode { get; }
    }
}
