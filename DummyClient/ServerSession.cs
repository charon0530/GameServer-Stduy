using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;
using System.Net;


namespace DummyClient
{
	//~에 연결된 세션
	class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            C_PlayerInfoReq packet = new C_PlayerInfoReq() { playerId =1001, name = "HELLO"};
			var skill = new C_PlayerInfoReq.Skill() { id = 999, level = 9, duration = 15.0f };
			skill.attributes.Add(new C_PlayerInfoReq.Skill.Attribute() { att = 77 });
			packet.skills.Add(skill);

			packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 101, level = 1, duration = 3.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 201, level = 2, duration = 4.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 301, level = 3, duration = 5.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 401, level = 4, duration = 6.0f });

            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = packet.Write();
                #region 주석처리
                //byte[] size = BitConverter.GetBytes(packet.size);
                //byte[] packetId = BitConverter.GetBytes(packet.packetId);
                //byte[] playerId = BitConverter.GetBytes(packet.playerId);

                //Array.Copy(size, 0, s.Array, s.Offset+count, 2);
                //count += 2;
                //Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
                //count += 2;
                //Array.Copy(playerId, 0, s.Array, s.Offset+count, 8);
                //count += 8;
                #endregion

                //궁금점? : 왜 큰 버퍼를 두고 잘라 사용해야하는가? 길이를 안다면 그만큼만 보내면 되는 것 아닌가? 
                // -> 가변적 + 메모리 낭비 줄이기
                if (s != null)
                    Send(s);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
