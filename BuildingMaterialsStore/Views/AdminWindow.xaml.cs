using System.Windows;
namespace BuildingMaterialsStore.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void DockPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    }
}
