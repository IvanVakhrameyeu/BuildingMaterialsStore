using BuildingMaterialsStore.ViewModels.AddWindow;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views.AddWindow
{
    public partial class AddGoods : Window
    {
        public AddGoods(int id)
        {
            InitializeComponent();
            DataContext = new GoodsW(id);
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
