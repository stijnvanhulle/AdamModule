using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AdamModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();
      
        public MainWindow()
        {
            InitializeComponent();
            Adam.Open();
            Sms.open();
           

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);


            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bw.RunWorkerAsync();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }
        AdamSocket wij = Adam.Open("172.23.49.102");
        AdamSocket zij = Adam.Open("172.23.49.102");
        
       

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
           

            Adam.Status status=Adam.Status.Released;
            Adam.Switch status2 = Adam.Switch.Off;
            bool infrarood = false;

            status= Adam.Read<Adam.Status>(Adam.PoortIn.GreenButton,wij);
            status2 = Adam.Read<Adam.Switch>(Adam.PoortIn.Switch,wij);
            infrarood = Adam.ReadInfrarood(Adam.Infrarood.Low, wij);

            if (infrarood == true)
            {
                Adam.Write(17, true, zij);
               
            }
            
            
            

            if (status == Adam.Status.Pressed || status == Adam.Status.Hold)
            {
                Adam.Write(17, true, zij);
                
            }
            else
            {
                

                

                if (status2 != Adam.Switch.None)
                {

                    if (status2 == Adam.Switch.On)
                    {
                        Adam.Write(17, true, zij);
                    }
                    else
                    {
                        Adam.Write(17, false, zij);
                        



                    }
                }
                else
                {
                    if (status == Adam.Status.Released)
                    {
                        Adam.Write(17, false, zij);
                    }
                    
                }

            }



            if (Sms.uitvoer.ToLower()=="lamp aan")
            {
                  Adam.Write(17, true, zij);
            }

            Console.WriteLine(status.ToString() + " - " + status2.ToString());
            //17 18

            
        }
    }
}
