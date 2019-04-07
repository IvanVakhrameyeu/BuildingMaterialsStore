using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views
{
    public partial class WindowAddPurchase : Window
    {
        public WindowAddPurchase(Purchases purchases, string NameCategory, string Name, string Description, double Price)
        {
            InitializeComponent();
            DataContext = new AddWindowModel( purchases,NameCategory,Name,Description,Price);
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
