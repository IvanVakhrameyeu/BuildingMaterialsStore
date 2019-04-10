using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views
{
    public partial class WindowAddPurchase : Window
    {
        public WindowAddPurchase(Purchases purchases, int AmountGoods)
        {
            InitializeComponent();
            DataContext = new AddPurchaseViewModel( purchases, AmountGoods);
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
