using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    class Listener
    {
        Socket _listenerSocket;
        Action<Socket> _onAcceptHandler;
        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
        {
            //문지기 핸드폰 기본 설정(통신방법설정) 하나의 휴대폰
            _listenerSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;
            //핸드폰 번호 설정(할당)
            _listenerSocket.Bind(endPoint);

            //영업 시작
            //backlog : 최대 대기수 = 바깥 의자수
            _listenerSocket.Listen(10);

            //비동기 작업을 적는 메모장과 같음
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            //"낚싯대 반응오면 쓰레드생성후 비동기작업 수행, 수행완료시 해당 쓰레드가 콜백함수 실행" => 던져놓은 낚싯대 수가 바로 최대 생성될 수 있는 쓰레드 수
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            // 낚싯대 던지기
            RegisterAccept(args); 
            
        }
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            //낚싯대 던져놓기(내생각엔 다른쓰레드에게 맡길 수 있으면(쓰레드 풀에 쓰레드가 남아있으면) true 
            //그러나, 바쁜상황이라 자기 자신 쓰레드가 처리해야하면 false
            bool pending = _listenerSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);
        }
    }
}
