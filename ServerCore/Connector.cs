﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public class Connector
    {
        Func<Session> _sessionFactory;
        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 1)
        {
            for (int i =0; i< count; i++)
            {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _sessionFactory = sessionFactory;

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
                args.RemoteEndPoint = endPoint;
                args.UserToken = socket;

                RegisterConnect(args);
                Thread.Sleep(1);
            }
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket =  args.UserToken as Socket;
            if (socket == null)
                return;

            bool pending = socket.ConnectAsync(args);
            if (pending == false)
                OnConnectCompleted(null, args);
        }

        void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.ConnectSocket);
                //RECEIVE할 준비를 마치고
                Console.WriteLine($"비교해보기 {args.ConnectSocket == args.UserToken}");
                session.OnConnected(args.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine($"OnConnectCompleted Fail: {args.SocketError}");
            }
        }
    }
}
