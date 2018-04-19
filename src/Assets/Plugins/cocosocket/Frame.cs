using System;
using System.Text;

namespace cocosocket4unity
{
	public class Frame
	{
		protected ByteBuf payload;
		protected bool end;

		protected Frame ()
		{
		}

		public Frame (int len)
		{
			this.payload = new ByteBuf (len);
			payload.WriteShort(0);
		}
		/**
		 * 获取缓冲区
		 **/ 
		public ByteBuf GetData()
		{
			return payload;
		}
		/**
		 * 写入一个字节
		 **/ 
		public Frame PutByte(byte c)
		{
			if (!end)
				payload.WriteByte(c);
			return this;
		}
		/**
		 * 写入一些字节
		 **/ 
		public Frame PutBytes(ByteBuf src)
		{
			if (!end)
				payload.WriteBytes(src);
			return this;
		}
        /**
         * 写入一些字节
         */ 
        public Frame PutBytes(byte[] src)
        {
            if (!end)
                payload.WriteBytes(src);
            return this;
        }
        /**
         * 写入加密
         */ 
        public Frame PutBytes(byte[] src,byte[] ks)
        {
            if (!end) 
            {
                xor(src, ks);
                payload.WriteBytes(src);
            }
            return this;
        }
		/**
		 * 写入整形
		 **/ 
		public Frame PutInt(int s)
		{
			if (!end)
				payload.WriteInt(s);
			return this;
		}
		/**
		 * 写入短整型
		 **/ 
		public Frame PutShort(short s)
		{
			if (!end)
				payload.WriteShort(s);
			return this;
		}
		/**
		 * 写入一个字符串
		 **/ 
		public Frame PutString(string s)
		{
			if (!end)
				payload.WriteUTF8(s);
			return this;
		}
		/**
		 * 写入一个字符串
		 **/ 
		public Frame PutString(string s,byte[] ks)
		{
			if (!end)
			{
				byte[] content = Encoding.UTF8.GetBytes (s.ToCharArray ());
				this.payload.WriteShort ((short)content.Length);
				xor (content, ks);
				this.payload.WriteBytes (content);
			}
			return this;
		}

		/**
		 * 封包
		 **/ 
		public virtual void End()
		{
			ByteBuf bb = payload;
			int reader = bb.ReaderIndex();
			int writer = bb.WriterIndex();
		    int l = writer - reader - 2; //数据长度
			bb.WriterIndex(reader);
			bb.WriteShort((short)l);
			bb.WriterIndex(writer);
			end = true;
		}
		/**
		 * 是否已经封包
		 **/ 
		public bool IsEnd()
		{
			return end;
		}
		/**
		 * 设置end标示
		 **/ 
		public void SetEnd(bool e)
		{
			if (e)
			{
				this.End();
			} else
			{
				this.end = e;
			}
		}
		/**
	 * 取异或
	 *
	 * @param bs
	 * @param ks
	 */
		public static void xor(byte[] bs, byte[] ks)
		{
			if (ks != null && ks.Length > 0)
			{
				for (int i = 0; i < bs.Length; i++)
				{
					bs[i] = (byte) (bs[i] ^ ks[i % ks.Length]);
				}
			}
		}
	}
}

