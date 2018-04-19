using System;

namespace cocosocket4unity
{
	public interface Protocol
	{
		ByteBuf TranslateFrame (ByteBuf src); 
	    int HeaderLen();
	}
}

