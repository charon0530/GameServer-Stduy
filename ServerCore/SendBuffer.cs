using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    //말 그대로 헬퍼 버퍼의 생성 교체 등을 담당함
    public class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => { return null; });

        public static int ChunkSize { get; set; } = 4096 * 100;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            if (CurrentBuffer.Value.FreeSize < reserveSize)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }
    //[][][][][u][][][][][]
    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize = 0;
        
        public int FreeSize { get { return _buffer.Length - _usedSize; } }
        public SendBuffer(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }
        //reserveSize = 한번에 보낼 패킷의 MAX값
        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;

            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;

        }
    }
}
