using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamModule
{
    public class Adam
    {
        public static AdamSocket Open(){
            AdamSocket socket = new AdamSocket();
            socket.Connect(AdamType.Adam6000, "172.23.49.102", System.Net.Sockets.ProtocolType.Tcp);
            string error = socket.LastError.ToString();
            if (socket == null) throw new Exception(error);

            return socket;
        }

        public static void Write(AdamSocket socket)
        {
            Modbus bus = new Modbus(socket);
            string address= bus.Address.ToString();

            socket.Send()
        }


        public static void Read(AdamSocket socket)
        {
            Modbus bus = new Modbus(socket);
            bool[] test;
            bus.ReadCoilStatus(0, 5, out test);

            var tes = test;
        }

        public static string Error(AdamSocket socket)
        {
            return socket.LastError.ToString();
        }
       


    }
}
