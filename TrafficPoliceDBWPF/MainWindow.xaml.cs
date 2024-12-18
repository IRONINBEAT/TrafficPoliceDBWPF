using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrafficPoliceAccidentsDLL;
using TrafficPoliceAddUserDLL;
using TrafficPoliceChangePasswordDLL;
using TrafficPoliceDirectoryManagementDLL;
using TrafficPoliceQueryDLL;
using TrafficPoliceUserAbilitiesDLL;
using TrafficPoliceVehiclesDLL;

namespace TrafficPoliceDBWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string userName)
        {
            InitializeComponent();

            var window = new UserAbilitiesWindow();
            window.Show();
        }
    }
}