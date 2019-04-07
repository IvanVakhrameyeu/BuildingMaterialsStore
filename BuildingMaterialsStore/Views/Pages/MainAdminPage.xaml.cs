using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class MainAdminPage : Page
    {
        public MainAdminPage(string section)
        {
            InitializeComponent();
            DataContext = new EmployeeViewModel(section);
        }
    }
}
