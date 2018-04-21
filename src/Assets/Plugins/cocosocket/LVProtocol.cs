using System;

namespace cocosocket4unity
{
	public class LVProtocol : Protocol
	{
		private int status;
		private int h;
		private int l;
		private short len;
		private ByteBuf frame;

		public LVProtocol ()
		{

		}
		/**
		 * 分帧逻辑
		 * 
		 **/ 
		public ByteBuf TranslateFrame(ByteBuf src)
		{
			while (src.ReadableBytes() > 0)
			{
				switch (status)
				{
				case 0:
					h = src.ReadByte();
					status = 1;
					break;
				case 1:
					l = src.ReadByte();
					len = (short)(((h << 8)&0x0000ff00) | (l));
					frame = new ByteBuf(len + 2);
					frame.WriteShort(len);
					status = 2;
					break;
				case 2:
				    int min=frame.WritableBytes();
					min=src.ReadableBytes()<min?src.ReadableBytes():min;
					if(min>0)
					{
					frame.WriteBytes(src,min);
					}
					if (frame.WritableBytes() <= 0)
					{
						status = 0;
						return frame;
					}
					break;
				}
			}
			return null;
		}
		/**
		 * 头部长度
		 * 
		 */ 
		public int HeaderLen()
		{
			return 2;
		}
	}
}

