using BuildingMaterialsStore.ViewModels;
using System.Windows.Controls;


namespace BuildingMaterialsStore.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        public Client(string section)
        {
            InitializeComponent();
            DataContext = new StorageViewModel(section);
        }
    }
}
