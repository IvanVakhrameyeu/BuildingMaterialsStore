using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views;

namespace BuildingMaterialsStore
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            WindowAuthorization windowAithorization = new WindowAuthorization();
            windowAithorization.ShowDialog();
            if(AuthorizationSettings.Access== "Middle")
            InitializeComponent();
            else
            {
                if (AuthorizationSettings.Access == "High")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.ShowDialog();
                    Close();
                }
                else
                {
                    Close();
                }
            }
        }
        private void DockPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
    }
}
