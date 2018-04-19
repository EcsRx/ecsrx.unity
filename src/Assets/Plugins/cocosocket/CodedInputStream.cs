/**
 * google 的varint算法
 * 
 */ 
using System;
namespace cocosocket4unity
{
	public class CodedInputStream
	{
		
		private sbyte[] buffer;
		private int bufferSize;
		private int bufferSizeAfterLimit;
		private int bufferPos;
		private int totalBytesRetired;
		private int currentLimit = int.MaxValue;
		private static  int DEFAULT_SIZE_LIMIT = 64 << 20;  // 64MB
		private int sizeLimit = DEFAULT_SIZE_LIMIT;

		public static CodedInputStream newInstance( sbyte[] buf)
		{
			return newInstance(buf, 0, buf.Length);
		}
		
		public static CodedInputStream newInstance( sbyte[] buf,  int off,  int len)
		{
			CodedInputStream result = new CodedInputStream(buf, off, len);
            result.pushLimit(len);
		
			return result;
		}
		
		public int readRawVarint32()
		{
			sbyte tmp = readRawByte();
			if (tmp >= 0)
			{
				return tmp;
			}
			int result = tmp & 0x7f;
			if ((tmp = readRawByte()) >= 0)
			{
				result |= (tmp << 7);
			} else
			{
				result |= ((tmp & 0x7f) << 7);
				if ((tmp = readRawByte()) >= 0)
				{
					result |= (tmp << 14);
				} else
				{
					result |= ((tmp & 0x7f) << 14);
					if ((tmp = readRawByte()) >= 0)
					{
						result |= (tmp << 21);
					} else
					{
						result |= ((tmp & 0x7f) << 21);
						result |= ((tmp = readRawByte()) << 28);
						if (tmp < 0)
						{
							// Discard upper 32 bits.
							for (int i = 0; i < 5; i++)
							{
								if (readRawByte() >= 0)
								{
									return result;
								}
							}

						}
					}
				}
			}
			return result;
		}
		
		private CodedInputStream( sbyte[] buffer,  int off,  int len)
		{
			this.buffer = buffer;
			bufferSize = off + len;
			bufferPos = off;
			totalBytesRetired = -off;
		}
		
		public int pushLimit(int byteLimit)
		{
			byteLimit += totalBytesRetired + bufferPos;
		    int oldLimit = currentLimit;
			currentLimit = byteLimit;
			recomputeBufferSizeAfterLimit();
			return oldLimit;
		}
		
		private void recomputeBufferSizeAfterLimit()
		{
			bufferSize += bufferSizeAfterLimit;
		    int bufferEnd = totalBytesRetired + bufferSize;
			if (bufferEnd > currentLimit)
			{
				// Limit is in current buffer.
				bufferSizeAfterLimit = bufferEnd - currentLimit;
				bufferSize -= bufferSizeAfterLimit;
			} else
			{
				bufferSizeAfterLimit = 0;
			}
		}
		
		private bool refillBuffer( bool mustSucceed) 
		{
			if (totalBytesRetired + bufferSize == currentLimit)
			{
				return false;
			}
			totalBytesRetired += bufferSize;
			bufferPos = 0;
			bufferSize = -1;
			if (bufferSize == -1)
			{
				bufferSize = 0;
				return false;
			} else
			{
				recomputeBufferSizeAfterLimit();
			    int totalBytesRead = totalBytesRetired + bufferSize + bufferSizeAfterLimit;
				return true;
			}
		}
		
		public sbyte readRawByte()
		{
			if (bufferPos == bufferSize)
			{
				refillBuffer(true);
			}
			return buffer[bufferPos++];
		}
	}
}

