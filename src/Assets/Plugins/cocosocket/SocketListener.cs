using System;

namespace cocosocket4unity
{
	public abstract  class SocketListener
	{
		abstract public void OnMessage(USocket us,ByteBuf bb);
		abstract public void OnClose(USocket us,bool fromRemote);
		abstract public void OnIdle(USocket us);
		abstract public void OnOpen(USocket us);
		abstract public void OnError(USocket us,string err);
	}
}

