/**
 * varint32头算法的frame
 */ 
using System;
using System.Text;

namespace cocosocket4unity
{
	public class Varint32Frame : Frame
	{

		public Varint32Frame (int len)
		{
			this.payload = new ByteBuf (len);
		}

		public Varint32Frame copy()
		{
			Varint32Frame fd = new Varint32Frame(1);
			fd.end = end;
			if (payload != null)
			{
				fd.payload = payload.Copy();
			}
			return fd;
		}
		/**
		 * 封包
		 */ 
		public override void End()
		{
			if (!this.end)
			{
		       ByteBuf bb = payload;
		       int reader = bb.ReaderIndex();
	           int writer = bb.WriterIndex();
			   int l = writer - reader;//数据长度
			   ByteBuf tar = new ByteBuf(l + 5);
			   WriteVarint32(tar, l);
			   tar.WriteBytes(bb);
			   payload = tar;
			   this.end = true;
			}
		}
/**
 * 写入一个varint32
 */ 
		public static ByteBuf WriteVarint32(ByteBuf tar, int v)
		{
				int len = CodedOutputStream.computeRawVarint32Size(v);
				CodedOutputStream headerOut = CodedOutputStream.newInstance(tar, len);
				headerOut.writeRawVarint32(v);
				headerOut.flush();
				return tar;
		}

	}
}

