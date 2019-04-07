using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views
{
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
