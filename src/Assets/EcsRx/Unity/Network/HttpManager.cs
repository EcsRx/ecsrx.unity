using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class HttpManager : IInitializable, IDisposable
    {
        private IList<HttpRequest> httpRequests;

        [Inject]
        private HttpRequest.Factory factory;
        public void Initialize()
        {
            httpRequests = new List<HttpRequest>();
        }

        public HttpRequest Create(string url)
        {
            return factory.Create(url);
        }

        public void Dispose()
        {
            httpRequests.Clear();
        }
    }
}
