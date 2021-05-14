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
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public override void OnConnected(EndPoint endPoint)
        {
            //쓰레드 수
            //Console.WriteLine($"thread count : {System.Threading.ThreadPool.ThreadCount}");
            Console.WriteLine($"OnConnected : {endPoint}");

            Program.Room.Push(() => Program.Room.Enter(this));
           


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
            SessionManager.Instance.Remove(this);
            if (Room != null)
            {
                GameRoom room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
           // Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
