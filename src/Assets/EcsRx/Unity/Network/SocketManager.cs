using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cocosocket4unity;
using ModestTree;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class SocketManager : IInitializable, IDisposable
    {
        private IList<SocketChannel> channels;
        [Inject]
        private SocketChannel.Factory factory;
        public SocketChannel DefaultChannel => channels.FirstOrDefault();

        public void Initialize()
        {
            channels = new List<SocketChannel>();
        }

        public SocketChannel Create(string ip, int port)
        {
            var channel = factory.Create(ip, port);
            channels.Add(channel);
            return channel;
        }

        public void Dispose()
        {
            channels.ForEach(channel => channel.Close());
            channels.Clear();
        }
    }
}
