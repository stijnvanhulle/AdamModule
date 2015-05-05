using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.template.ui.ViewModel
{
    public class PageOneVM : ObservableObject, IPage
    {

        ApplicationVM vm = App.Current.MainWindow.DataContext as ApplicationVM;
        public string Name
        {
            get { return "First page" ;}
        }
            
       
        public ICommand cmdToegang
        {
            get { return new RelayCommand(Access); }
        }

        private void Access()
        {
            vm.ChangePage(new PageAccessVM());
           
        }
	    


    }
}
