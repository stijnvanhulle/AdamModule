using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SmsModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            port = new SerialPort("COM3", 9600, Parity.None, 8);
            port.RtsEnable = true;
            port.Open();
            port.Handshake = Handshake.None;
           

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        string uitvoer = "";
        private void timer_Tick(object sender, EventArgs e)
        {
            string tekst = port.ReadExisting();
            Console.Write(tekst);

            uitvoer += tekst;
            if (uitvoer!="")
            {
                ReadMessage(uitvoer);
            }
            

            txtUitvoer.Text = uitvoer;
        }

        private string[] ReadMessage(string uitvoer)
        {
            uitvoer="\r\n+CMT: \"+32497251615\",,\"15/04/23,11:24:01+08\"\r\nA\r\n";
            int lengte = uitvoer.Length;
            int cmt = uitvoer.IndexOf('+');
            string lijst = uitvoer.Substring(cmt, lengte - cmt);
            string[] test=  lijst.Split('"');
            test[test.Length - 1] = test[test.Length - 1].Substring(2, test[test.Length - 1].Length - 2 - 2);
            return test;
        }


        char aanhaling = '\"';
        char ctrlz = (char)26;
        string enter = "\r\n";
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            

            SendTo("+32476043104","dag thomas");
            uitvoer = "";

            //string toWrite = txtTekst.Text.ToString();
            //port.Write(toWrite+enter); //van tekstbox

            ReadNewMessage();

            

            
        }

        private void ReadNewMessage()
        {
            string read = "at+cmgl=" + aanhaling + "REC UNREAD" + aanhaling + enter;
            port.Write(read);
        }

        private void SendTo(string nummer, string message)
        {
            string tekst = "at+cmgs=" + aanhaling + nummer + aanhaling;
            tekst += enter;
            tekst += message;
            tekst += ctrlz;
            port.Write(tekst);
        }
    }
}
