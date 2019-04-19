using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels.Pages;
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
        static public List<string> firms { get; set;  }
        static private string _selectFirm = null;
        static public string SelectFirm
        {
            get { return _selectFirm; }
            set
            {
                if (_selectFirm == value) return;
                _selectFirm = value;
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
        private Page GoodsIssuePage;

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
            GoodsIssuePage = new GoodsIssue();
        }
        async private void awayMethods()
        {
            await Task.Run(() => FillListFirms());
        }
        private void FillListFirms()
        {
            if (firms == null)
            {
                SqlConnection con;
                SqlCommand com;
                DataTable dt = new DataTable();
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("select distinct FirmName  from Firms", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
                    con.Close();
                }
                firms = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    firms.Add(dr[0].ToString());
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
                case 10: { CurrentPage = GoodsIssuePage; break; }
                default: { CurrentPage = null; break; }
            }
        }
    }
}
