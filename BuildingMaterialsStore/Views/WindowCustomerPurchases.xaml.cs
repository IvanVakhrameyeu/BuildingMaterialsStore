using BuildingMaterialsStore.Models;
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
    /// Логика взаимодействия для WindowCustomerPurchases.xaml
    /// </summary>
    public partial class WindowCustomerPurchases : Window
    {
        public WindowCustomerPurchases(List<Purchases> purchases)
        {
            InitializeComponent();
            DataContext = new PurchasesViewModel(purchases);
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
