using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views
{
    public partial class WindowCustomerPurchases : Window
    {
        public WindowCustomerPurchases()
        {
            InitializeComponent();
            DataContext = new PurchasesViewModel();
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
