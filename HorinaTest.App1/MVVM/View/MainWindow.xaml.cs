using System.Windows;
using HorinaTest.App1.MVVM.ViewModel;

namespace HorinaTest.App1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowVM { get; set; }

        public MainWindow()
        {
           DataContext = _mainWindowVM = new MainWindowViewModel();

            InitializeComponent();
        }
    }
}
