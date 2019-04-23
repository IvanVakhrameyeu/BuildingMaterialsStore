using BuildingMaterialsStore.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.Views
{
    public partial class WindowAddStorage : Window
    {
        public WindowAddStorage()
        {
            InitializeComponent();
            DataContext = new AddStorageVM();
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
