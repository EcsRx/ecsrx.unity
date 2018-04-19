using LitJson;
using ProtoBuf;
using ProtoBuf.Serializers;
using System.Collections.Generic;
using System;
using System.IO;
using protocol;
using UnityEngine;
using Game;
using System.Reflection;

namespace cocosocket4unity {
	public class MyListner : SocketListener {
		public MyListner() {
			
		}

		public override void OnMessage(USocket us, ByteBuf bb)
		{
			bb.ReaderIndex (us.getProtocol().HeaderLen());
			short cmd = bb.ReadShort();
			byte[] bs = bb.GetRaw();
			Statics.SetXor(bs, bb.ReaderIndex());
			MemoryStream stream = new MemoryStream(bs, bb.ReaderIndex(), bb.ReadableBytes());
            object obj = ProtoBuf.Serializer.NonGeneric.Deserialize(MessageQueueHandler.GetProtocolType(cmd), stream);
            FieldInfo success = obj.GetType().GetField("success");
            if (success != null) { 
                if ((bool)success.GetValue(obj) == true) {
                    MessageQueueHandler.PushQueue(cmd, obj);
                } else {
                    FieldInfo info = obj.GetType().GetField("info");
                    if (info != null) {
                        Debug.LogWarning("下行出错, cmd=" + cmd + ", type=" + MessageQueueHandler.GetProtocolType(cmd).ToString());
                        MessageQueueHandler.PushError(info.GetValue(obj).ToString());
                    }
                }
            }
		}
		/**
		 * 
		 */ 
		public override  void OnClose(USocket us, bool fromRemote)
		{
			MessageQueueHandler.PushError(fromRemote ? "与服务器连接已断开" : "关闭连接");
		}
		public override  void OnIdle(USocket us)
		{
			Debug.LogWarning("连接超时");
		}
		public override  void OnOpen(USocket us)
		{
			Debug.LogWarning("连接建立, ip=" + us.getIp());
		}
		public override void OnError(USocket us, string err)
		{
			Debug.LogWarning("异常:" + err);
		}
	}
}