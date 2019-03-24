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
    /// Логика взаимодействия для WindowAddPurchase.xaml
    /// </summary>
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
