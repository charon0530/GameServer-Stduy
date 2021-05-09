//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Server
//{
//    //class의 public은 밖에서 아느냐 모르느냐
//    public class myCLS
//    {
//        public myGen<inGen> tt = new myGen<inGen>();
//    }
//    public class myGen<T> 
//    {
       
//    }
//    public class inGen
//    {

//    }
//    class MySessionHD
//    {
//        public void OnStart(MySocket mySocket)
//        {
//            mySocket._sockeNum += 1;
//            Console.WriteLine("OnSTART!");
//            Console.WriteLine($"socket num increased! {mySocket._sockeNum}");

//        }
//    }
//    class Knight
//    {
//        public int hp;
//        public int MP
//        {
//            get; set;
//        }
//    }
//    class Class2
//    {
//        static void Main(string[] args)
//        {
//            Knight kn = new Knight { hp = 60 , MP = 70};
//            Console.WriteLine($"kngith hp : {kn.hp} mp : {kn.MP}");
//            Class1 c1 = new Class1();
//            c1.Start(new MySessionHD());
//        }
//    }
//}
