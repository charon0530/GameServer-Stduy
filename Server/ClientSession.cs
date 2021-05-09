using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{

	class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            //쓰레드 수
            Console.WriteLine($"thread count : {System.Threading.ThreadPool.ThreadCount}");
            Console.WriteLine($"OnConnected : {endPoint}");

            //Packet packet = new Packet() { size=100, packetId=10};
            //
            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(packet.size);
            //byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            //ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);
            //
            //Send(sendBuff);

            Thread.Sleep(5000);
            Disconnect();

            //입출력테스트
            //Thread cur_thread = Thread.CurrentThread;
            //Console.WriteLine("thread = {0}", cur_thread.ManagedThreadId);
            //byte[] mysendBuff = Encoding.UTF8.GetBytes($"Hello world! {cur_thread.ManagedThreadId}");
            //Send(mysendBuff);

        }
        public override void OnRecvPacket(ArraySegment<byte> buffer) //buffer = full packet
        {
			PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
