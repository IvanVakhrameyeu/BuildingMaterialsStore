using BuildingMaterialsStore.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BuildingMaterialsStore.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowAddCustomer.xaml
    /// </summary>
    public partial class WindowAddCustomer : Window
    {
        public WindowAddCustomer()
        {
            InitializeComponent();
            DataContext = new AddCustomerViewModel();
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
