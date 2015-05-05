using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AdamModule
{
    public class Adam
    {

        public enum Status {None, Pressed, Released, Hold}
        public enum Switch {None,On, Off }
        public enum PoortIn {BlackButton=4,GreenButton=3,Switch=1 }
        public enum Infrarood { Low=4,Medium=5,High=6}
        public enum Lights { First=18,Second=19,Thirth=20,Fourth=21}
        public enum Uitvoer { Lamp=17,Vens=17}

        public static AdamSocket Open(string adres="172.23.49.102"){
            AdamSocket socket = new AdamSocket();
            //socket.Connect(AdamType.Adam6000, "172.23.49.102", System.Net.Sockets.ProtocolType.Tcp);

            socket.Connect(adres,System.Net.Sockets.ProtocolType.Tcp,502);
            string error = socket.LastError.ToString();
            if (socket == null) throw new Exception(error);


        
            return socket;
        }

        public static void Write(int poort,bool status,AdamSocket socket)
        {

            //socket.DigitalOutput(00017).SetValue(00017, true);

            if (socket != null)
            {
                Modbus m = new Modbus(socket);
                m.ForceSingleCoil(poort, status);
               

               
            }
           
           
         
           
           
        }

        #region status

        private static Status vorigeStatus= Status.None;
        private static List<Status> statussen = new List<Status>();
        public static T Read<T>(PoortIn adres, AdamSocket socket=null)
        {
            if (adres == PoortIn.Switch)
            {
                Switch s=ReadSwitch(adres,socket);
                return (T)Convert.ChangeType(s, typeof(T));
            } 

            bool[] test;
            if (socket == null) socket = Open();
            Modbus m = new Modbus(socket);
            Status waarde=Status.None;
            m.ReadCoilStatus((int) adres, 1, out test);
           

            if (vorigeStatus == Status.None)
            {
                if (test[0] == true) vorigeStatus = Status.Pressed;
                if (test[0] == false) vorigeStatus = Status.Released;

                waarde= vorigeStatus;
            }
            else
            {
                if (statussen.Count > 1 && statussen.Last<Status>()== Status.Pressed)
                {
                    waarde = Status.Hold;
                }

                if (vorigeStatus == Status.Released && test[0] == true)
                {
                    vorigeStatus = test[0]==true ? Status.Pressed:Status.Released;
                    statussen.Add(Status.Pressed);
                    waarde= Status.Pressed;
                   
                }

                if (vorigeStatus == Status.Pressed && test[0] == false)
                {
                    vorigeStatus = test[0] == true ? Status.Pressed : Status.Released;
                    statussen.Add(Status.Released);
                    waarde= Status.Released;

                }
            }



            return (T)Convert.ChangeType(waarde, typeof(T));
        }

        #endregion

        #region switch

        private static Switch vorigeSwitch = Switch.None;
        private static Switch ReadSwitch(PoortIn adres,AdamSocket socket=null)
        {
            bool[] test;
            bool[] test2;
            Switch waarde=Switch.None;
            if (socket == null) socket = Open();
            Modbus m = new Modbus(socket);
           
            m.ReadCoilStatus((int)adres, 1, out test);
            m.ReadCoilStatus((int)adres + 1, 1, out test2);

            if (vorigeSwitch ==Switch.None)
            {
                if (test[0] == true && test2[0] == test[0]) vorigeSwitch = Switch.On;
                if (test[0] == false && test[0] == test2[0]) vorigeSwitch = Switch.Off;

                waarde= vorigeSwitch;
            }
            else
            {
                if (test[0] == test2[0] && test[0] == true && vorigeSwitch == Switch.Off)
                {
                    vorigeSwitch = test[0] == true ? Switch.On : Switch.Off;

                    waarde = Switch.On;

                }

                if (test[0] == test2[0] && test[0] == false && vorigeSwitch == Switch.On)
                {
                    vorigeSwitch = test[0] == true ? Switch.On : Switch.Off;

                    waarde = Switch.Off;

                }
            }

            

            
           

           return waarde;

        }

        #endregion

        #region infrarood

        public static bool ReadInfrarood(Infrarood startAdres, AdamSocket socket = null)
        {
            bool[] test;
            bool[] test2;
            bool[] test3;
            bool waarde = false;
            if (socket == null) socket = Open();
            Modbus m = new Modbus(socket);

            m.ReadCoilStatus((int)startAdres, 1, out test);
            m.ReadCoilStatus((int)startAdres + 1, 1, out test2);
            m.ReadCoilStatus((int)startAdres + 2, 1, out test3);

            if (test[0]==true || test2[0]==true || test3[0]==true )
            {
                waarde = true;
            }

            return waarde;

        }

        #endregion

        public static string Error(AdamSocket socket)
        {
            return socket.LastError.ToString();
        }
       


    }
}
