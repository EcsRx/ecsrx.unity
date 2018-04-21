/**
 * google varint算法
 */ 
using System;
namespace cocosocket4unity
{
	public class CodedOutputStream
	{
		private  byte[] buffer;
		private  int limit;
		private int position;
		private  ByteBuf output;
		
		private CodedOutputStream( ByteBuf output,  byte[] buffer)
		{
			this.output = output;
			this.buffer = buffer;
			position = 0;
			limit = buffer.Length;
		}
		
		/**
   * Create a new {@code CodedOutputStream} wrapping the given
   * {@code OutputStream} with a given buffer size.
   *
   * @param output
   * @param bufferSize
   * @return
   */
		public static CodedOutputStream newInstance( ByteBuf output,  int bufferSize)
		{
			return new CodedOutputStream(output, new byte[bufferSize]);
		}
		
		/**
   * Compute the number of bytes that would be needed to encode a varint.
   * {@code value} is treated as unsigned, so it won't be sign-extended if
   * negative.
   *
   * @param value
   * @return
   */
		public static int computeRawVarint32Size(int value)
		{
			if ((value & (0xffffffff << 7)) == 0)
			{
				return 1;
			}
			if ((value & (0xffffffff << 14)) == 0)
			{
				return 2;
			}
			if ((value & (0xffffffff << 21)) == 0)
			{
				return 3;
			}
			if ((value & (0xffffffff << 28)) == 0)
			{
				return 4;
			}
			return 5;
		}
		
		/**
   * Encode and write a varint. {@code value} is treated as unsigned, so it
   * won't be sign-extended if negative.
   *
   * @param value
   * @throws java.io.IOException
   */
		public void writeRawVarint32(int value)
		{
			while (true)
			{
				if ((value & ~0x7F) == 0)
				{
					writeRawByte(value);
					return;
				} else
				{
					writeRawByte((value & 0x7F) | 0x80);
					value= rightMove(value,7);//>>>
				}
			}
		}
		public static int rightMove(int x, int y) {
			int mask = 0x7fffffff;
			for (int i = 0; i < y; i++) {
				x >>= 1;
				x &= mask;
			}
			return x;
		}
		/**
   * Write a single byte, represented by an integer value.
   *
   * @param value
   * @throws java.io.IOException
   */
		public void writeRawByte( int value) 
		{
			writeRawByte((byte) value);
		}
		
		/**
   * Write a single byte.
   *
   * @param value
   * @throws java.io.IOException
   */
		public void writeRawByte( byte value)
		{
			if (position == limit)
			{
				refreshBuffer();
			}
			buffer[position++] = value;
		}
		
		/**
   * Internal helper that writes the current buffer to the output. The buffer
   * position is reset to its initial value when this returns.
   */
		private void refreshBuffer()
		{
			// Since we have an output stream, this is our buffer
			// and buffer offset == 0
			output.WriteBytes(buffer, 0, position);
			position = 0;
		}
		
		/**
   * Flushes the stream and forces any buffered bytes to be written. This does
   * not flush the underlying OutputStream.
   *
   * @throws java.io.IOException
   */
		public void flush()
		{
			refreshBuffer();
		}
	}
}

