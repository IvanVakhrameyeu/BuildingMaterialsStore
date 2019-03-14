using BuildingMaterialsStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BuildingMaterialsStore.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainStorage.xaml
    /// </summary>
    public partial class MainStorage : Page
    {
        public MainStorage(string section)
        {
            InitializeComponent();
            DataContext = new StorageViewModel(section);
        }
    }
}
