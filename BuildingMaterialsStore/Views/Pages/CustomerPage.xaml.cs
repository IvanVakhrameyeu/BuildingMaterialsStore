using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            try
            {
                InitializeComponent();
                DataContext = new CustomerViewModel();
            }
            catch { }
        }
    }
}
