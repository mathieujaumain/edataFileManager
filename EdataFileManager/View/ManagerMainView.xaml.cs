using System.Windows;
using EdataFileManager.ViewModel;
using EdataFileManager.ViewModel.Edata;

namespace EdataFileManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ManagerMainViewModel();
        }

    }
}
