using BuildingMaterialsStore.ViewModels.Pages;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class StoragePage : Page
    {
        public StoragePage()
        {
            InitializeComponent();
            DataContext = new StorageVM();
        }
    }
}
