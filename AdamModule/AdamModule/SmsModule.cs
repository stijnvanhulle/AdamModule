using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AdamModule
{
    public class Sms
    {
        public static SerialPort port;
        private static DispatcherTimer timer;
        public static string uitvoer = "";
 
        public static void open(){
            port = new SerialPort("COM3", 9600, Parity.None, 8);
            port.RtsEnable = true;
            port.Open();
            port.Handshake = Handshake.None;


            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            ReadNewMessage();

            string tekst = port.ReadExisting();
            Console.Write(tekst);

            uitvoer += tekst;
            if (uitvoer != "")
            {
                string[] uit= ReadMessage(uitvoer);
                if (uit!=null)
                {
                    uitvoer = uit[uit.Length - 1];

                }
                else
                {
                    uitvoer = "";
                }

               
            }
        }

       
        

        private static string[] ReadMessage(string uitvoer)
        {
            //uitvoer="\r\n+CMT: \"+32497251615\",,\"15/04/23,11:24:01+08\"\r\nA\r\n";
            int lengte = uitvoer.Length;
            int cmt = uitvoer.IndexOf('+');
            string lijst = uitvoer.Substring(cmt, lengte - cmt);
            string[] test=  lijst.Split('"');
            test[test.Length - 1] = test[test.Length - 1].Substring(2, test[test.Length - 1].Length - 2 - 2);
            if (test[test.Length-1]=="\nOK")
            {
                return null;
            }
            else
            {
                return test;

            }
           
        }


        char aanhaling = '\"';
        char ctrlz = (char)26;
        string enter = "\r\n";

        //SendTo("+32476043104","dag thomas");
        //ReadNewMessage();
        

        private static void ReadNewMessage()
        {
            char aanhaling = '\"';
            char ctrlz = (char)26;
            string enter = "\r\n";

            string read = "at+cmgl=" + aanhaling + "REC UNREAD" + aanhaling + enter;
            port.Write(read);
        }

        public static void SendTo(string nummer, string message)
        {
            char aanhaling = '\"';
            char ctrlz = (char)26;
            string enter = "\r\n";

            string tekst = "at+cmgs=" + aanhaling + nummer + aanhaling;
            tekst += enter;
            tekst += message;
            tekst += ctrlz;
            port.Write(tekst);
        }
    }
}
