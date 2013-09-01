using System.Windows;
using EdataFileManager.ViewModel;

namespace EdataFileManager
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
