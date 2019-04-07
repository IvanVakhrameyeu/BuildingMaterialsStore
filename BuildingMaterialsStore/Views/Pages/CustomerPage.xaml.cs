using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class CustomerPage : Page
    {
        public CustomerPage(string section)
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(section);
        }
    }
}
