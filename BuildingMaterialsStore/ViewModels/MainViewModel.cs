using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views;
using BuildingMaterialsStore.Views.Pages;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class MainViewModel : ViewModel
    {
        public ICommand QuitAplicationCommand { get; } = new DelegateCommand(p => Application.Current.Shutdown());
        public ICommand HelpAplicationCommand { get; }
        private WindowState _currentSate;
        public ICommand WindowStateCommand { get; }
        public ICommand ShoppingBasket { get; }
        public static bool isChange=false;
        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        public WindowState CurrentWindowState
        {
            get { return _currentSate; }
            set
            {
                _currentSate = value;
                OnPropertyChanged("CurrentWindowState");
            }
        }
        static public List<string> customers { get; set; }
        static private string _selectCustomer = null;
        static public string SelectCustomer
        {
            get { return _selectCustomer; }
            set
            {
                if (_selectCustomer == value) return;
                _selectCustomer = value;
                StorageViewModel.purchases = new List<Purchases>();
                PurchasesViewModel.InTotal = 0;
            }
        }
        private void OnCurrentWindowState(object p)
        {
            CurrentWindowState = WindowState.Minimized;
        }
        private Page MainStoragePage;
        private Page DecorationMaterialsPage;
        private Page GeneralConstructionPage;
        private Page RoofingMaterialsPage;
        private Page FacadeMaterialsPage;
        private Page ElectricianPage;
        private Page HardwareFastenersPage;
        private Page OvenMaterialsPage;
        private Page GardenPage;

        private Page CustomersPage;
        public MainViewModel()
        {
            HelpAplicationCommand = new DelegateCommand(OnHelpCommandExecuted);
            ShoppingBasket = new DelegateCommand(OnAddPurchaseCommandExecuted);
            awayMethods();
            changePages();
            CustomersPage = new CustomerPage();

            CurrentPage = MainStoragePage;
            WindowStateCommand = new DelegateCommand(OnCurrentWindowState);
        }
        /// <summary>
        /// обновление всех пунктов меню с товарами
        /// </summary>
        private void changePages()
        {
            MainStoragePage = new MainStorage("Главная");
            DecorationMaterialsPage = new MainStorage("Отделочные материалы");
            GeneralConstructionPage = new MainStorage("Общестроительные");
            RoofingMaterialsPage = new MainStorage("Кровельные материалы");
            FacadeMaterialsPage = new MainStorage("Фасадные материалы");
            ElectricianPage = new MainStorage("Электрика");
            HardwareFastenersPage = new MainStorage("Метизы и крепеж");
            OvenMaterialsPage = new MainStorage("Печные материалы");
            GardenPage = new MainStorage("Сад и огород");
        }
        async private void awayMethods()
        {
            await Task.Run(() => FillListCustomer());

        }
        private void FillListCustomer()
        {
            if (customers == null)
            {
                SqlConnection con;
                SqlCommand com;

                DataTable dt = new DataTable();
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();

                    using (com = new SqlCommand("select distinct CustLastName, CustFirstName from Customer", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
                    con.Close();
                }
                customers = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    customers.Add(dr[0].ToString() + " " + dr[1].ToString());
                }
            }
        }
        private void OnAddPurchaseCommandExecuted(object o)
        {
            new WindowCustomerPurchases(StorageViewModel.purchases).ShowDialog();
            if (isChange) { changePages(); isChange = false; CurrentPage = MainStoragePage; }
        }

        private void OnHelpCommandExecuted(object o)
        {
            try
            {
                Process.Start("Help.chm");
            }
            catch { MessageBox.Show("Справка не найдена"); }
        }
        private int _selectedIndex = -1;
        public int SeletedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                LoadedPage(_selectedIndex);
            }
        }
        /// <summary>
        /// загрузка страницы при выборе меню
        /// </summary>
        /// <param name="i"></param>
        private void LoadedPage(int i)
        {
            switch (i)
            {
                case 0: { CurrentPage = MainStoragePage; break; }
                case 1: { CurrentPage = DecorationMaterialsPage; break; }
                case 2: { CurrentPage = GeneralConstructionPage; break; }
                case 3: { CurrentPage = RoofingMaterialsPage; break; }
                case 4: { CurrentPage = FacadeMaterialsPage; break; }
                case 5: { CurrentPage = ElectricianPage; break; }
                case 6: { CurrentPage = HardwareFastenersPage; break; }
                case 7: { CurrentPage = OvenMaterialsPage; break; }
                case 8: { CurrentPage = GardenPage; break; }
                case 9: { CurrentPage = CustomersPage; break; }
                default: { CurrentPage = null; break; }
            }
        }
    }
}
