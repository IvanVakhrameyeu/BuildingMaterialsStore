using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class MainStorage : Page
    {
        public MainStorage(string section)
        {
            try
            {
                InitializeComponent();
                DataContext = new StorageViewModel(section);
            }
            catch { }
        }
    }
}
