using BuildingMaterialsStore.ViewModels.Pages;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Goods.xaml
    /// </summary>
    public partial class Goods : Page
    {
        public Goods()
        {
            InitializeComponent();
            DataContext = new GoodsVM();
        }
    }
}
