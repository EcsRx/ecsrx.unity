using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cocosocket4unity;
using EcsRx.Network;
using EcsRx.Serialize;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class SocketChannel
    {
        public string IP;
        public int Port;
        private USocket socket;
        private SocketListener listener;
        private ISocketProtocol socketProtocol;

        public Action<USocket> SocketOpenedEvent => listener.SocketOpenedEvent;

        public SocketChannel(string ip, int port, SocketListener listener, Protocol protocol, ISocketProtocol socketProtocol)
        {
            IP = ip;
            Port = port;
            this.socketProtocol = socketProtocol;
            this.listener = listener;
            socket = new USocket(listener, protocol);
        }

        public class Factory : Factory<string, int, SocketChannel>
        {
        }

        public bool IsConnect()
        {
            return socket.getStatus() == USocket.STATUS_CONNECTED;
        }

        public void Connect()
        {
            if (!IsConnect())
            {
                socket.Connect(IP, Port);
            }
        }

        public void Close()
        {
            if (socket.getStatus() != USocket.STATUS_CLOSED)
            {
                socket.Close();
            }
        }

        public void Send<T>(SocketRequestMessage<T> message)
        {
            if (socket.getStatus() == USocket.STATUS_CONNECTED)
            {
                Frame f = socketProtocol.EncodeMessage(message);
                socket.Send(f);
            }
        }

        public IObservable<T> Receive<T>()
        {
            return Observable.Create<T>(observer =>
            {
                Callback<T> handler = observer.OnNext;
                Messenger.AddListener<T>(typeof(T).FullName, handler);
                return Disposable.Create(() => Messenger.RemoveListener<T>(typeof(T).FullName, handler));
            });
        }
    }
}
