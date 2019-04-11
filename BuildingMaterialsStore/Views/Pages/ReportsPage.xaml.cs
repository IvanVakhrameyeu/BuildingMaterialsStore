using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            try
            {
                InitializeComponent();
                DataContext = new ReportViewModel();
            }
            catch { }
        }
    }
}
