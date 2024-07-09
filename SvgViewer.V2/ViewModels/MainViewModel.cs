using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvgViewer.V2.Models;
using SvgViewer.V2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SvgViewer.V2.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Card> Cards { get; set; } 

        public ICommand ClickCommand { get; set; }

        public MainViewModel()
        {
            Cards = [new Card("G:\\SvgSize\\1 (1).svg", "1 (1).svg"), new Card("G:\\SvgSize\\1 (1).svg", "1 (1).svg")];

            ClickCommand = new RelayCommand<Card>(HandleClick);
        }

        public void HandleClick(Card? card)
        {
            HandyControl.Controls.Growl.Success(SuccessGrowlInfo.Instance);
        }


    }
}
