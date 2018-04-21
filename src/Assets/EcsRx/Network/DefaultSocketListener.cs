using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cocosocket4unity;
using EcsRx.ErrorHandle;
using EcsRx.Events;
using EcsRx.Serialize;
using EcsRx.Unity.Exception;
using UnityEngine;

namespace EcsRx.Network
{
    public class DefaultSocketListener : SocketListener
    {
        private ISocketProtocol protocol;
        private IErrorHandle errorHandle;
       
        public DefaultSocketListener(ISocketProtocol protocol, IErrorHandle errorHandle)
        {
            this.protocol = protocol;
            this.errorHandle = errorHandle;
        }
        public override void OnMessage(USocket us, ByteBuf bb)
        {
            bb.ReaderIndex(us.getProtocol().HeaderLen());
            short cmd = bb.ReadShort();
            byte[] bs = bb.GetRaw();
            using (var stream = new MemoryStream(bs, bb.ReaderIndex(), bb.ReadableBytes()))
            {
                object obj = protocol.DecodeMessage(MessageQueueHandler.GetProtocolType(cmd), stream);
                MessageQueueHandler.PushQueue(cmd, obj);
            }
        }

        public override void OnClose(USocket us, bool fromRemote)
        {
            Debug.LogWarning(fromRemote ? "与服务器连接已断开" : "关闭连接");
            errorHandle.Handel(new HttpException(fromRemote ? "与服务器连接已断开" : "关闭连接",
                -1));
        }

        public override void OnIdle(USocket us)
        {
            Debug.LogWarning("连接超时");
            errorHandle.Handel(new HttpException("连接超时",
                -1));
        }

        public override void OnOpen(USocket us)
        {
            SocketOpenedEvent?.Invoke(us);
        }

        public override void OnError(USocket us, string err)
        {
            Debug.LogWarning("异常:" + err);
            errorHandle.Handel(new HttpException("异常:" + err,
                -1));
        }
    }
}
