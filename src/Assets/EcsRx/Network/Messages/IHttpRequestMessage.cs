using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IHttpRequestMessage : IRequestMessage
    {
        string Url { get; set; }
        string Path { get; set; }
    }
}
