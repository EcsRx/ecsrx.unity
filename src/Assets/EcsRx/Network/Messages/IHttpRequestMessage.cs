using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Net
{
    public interface IHttpRequestMessage : IRequestMessage
    {
        string Path { get; }
    }
}
