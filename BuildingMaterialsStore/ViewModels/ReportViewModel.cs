using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels.WordReports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class ReportViewModel : ViewModel
    {
        SqlConnection con;
        SqlCommand com;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;

        public ObservableCollection<Storage> storages { get; set; }
        public List<string> employees { get; set; }
        public List<string> firms { get; set; }
        private ObservableCollection<string> _namesCategory;
        private ObservableCollection<string> _names;

        public ObservableCollection<string> namesCategory{ get {  return _namesCategory;  } set { _namesCategory = value; } }
        public ObservableCollection<string> names { get { return _names; } set { _names = value; } }

        public ICollectionView view { get; set; }
        public ICommand PurchasesRepCommand { get; }
        public ICommand EmplRepCommand { get; }
        public ICommand EmplsRepCommand { get; }
        public ICommand CustRepCommand { get; }
        public ICommand ClearFilterCommand { get; }

        private string _selectedFirm = null;
        private string _selectedEmployee = null;
        private string _selectNameItem = null;
        private string _selectNameCategoryItem = null;
        private string _text = null;
        private Storage _selectItemDataGrid = null;
        public Storage SelectItemDataGrid
        {
            get { return _selectItemDataGrid; }
            set
            {
                if (_selectItemDataGrid == value)
                    return;

                _selectItemDataGrid = value;
            }
        }

        private DateTime _dateFrom= DateTime.Now;
        private DateTime _dateTo = DateTime.Now;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { _dateFrom = value; OnPropertyChanged("DateFrom"); }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { _dateTo = value; OnPropertyChanged("DateTo"); }
        }
        public string SelectedFirm
        {
            get { return _selectedFirm; }
            set
            {
                _selectedFirm = value;
            }
        }
        public string SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
            }
        }
        public string SelectNameItem
        {
            get { return _selectNameItem; }
            set
            {
                if (_selectNameItem == value) return;
                _selectNameItem = value;
                Filter();
            }
        }
        public string SelectNameCategoryItem
        {
            get { return _selectNameCategoryItem; }
            set
            {
                if (_selectNameCategoryItem == value) return;
                _selectNameCategoryItem = value;
                Filter();
                FillListNameWithCategory();
            }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                if (_text == null) return;
                Filter();
            }
        }
        public ReportViewModel()
        {
            asyncMainMethod();
            PurchasesRepCommand = new DelegateCommand(OnPurchasesCommandExecuted);
            EmplRepCommand = new DelegateCommand(OnEmplCommandExecuted);
            EmplsRepCommand = new DelegateCommand(OnEmplsCommandExecuted);
            CustRepCommand = new DelegateCommand(OnCustCommandExecuted);
            CustRepCommand = new DelegateCommand(OnCustCommandExecuted);
            ClearFilterCommand = new DelegateCommand(OnClearFilterCommandExecuted, OnClearFilterCommandCanExecuted);
        }
        /// <summary>
        /// заполнение всех comboBox
        /// </summary>
        private void asyncMainMethod()
        {
            try
            {
                FillListEmployee();
                FillListFirms();

                FillList();
                FillListNameCategory();
                FillListName();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// вывод покупок за данный период(удалить возможно)
        /// </summary>
        /// <param name="o"></param>
        private void OnPurchasesCommandExecuted(object o)
        {
            if (DateFrom > DateTo) { MessageBox.Show("Проверьте формат даты"); return; }

            string sql = "select FirmName, UNP, Store.[Count],  Store.CurrentDiscountAmount, TotalPrice, PurchaseDay " +
                "from Store " +
                "join Firms on(Store.FirmID= Firms.FirmID) " +
                "join Storage on(Store.StorageID= Storage.StorageID) " +
                "join Category on(Storage.CategoryID= Category.CategoryID) " +
                "where Store.StorageID = " + SelectItemDataGrid.idStorage + " " +
                "and PurchaseDay>= '" + DateFrom + "' and PurchaseDay<= '" + DateTo + "'";

            (new PurchasesRep()).writeClass(DateFrom, DateTo, "reportHistory", "товарам", SelectItemDataGrid.Name, SelectItemDataGrid.Price, SelectItemDataGrid.NameCategory, SelectItemDataGrid.Description, SelectItemDataGrid.Count, sql);
        }
        /// <summary>
        /// вывод отчета по работнику за данный период
        /// </summary>
        /// <param name="o"></param>
        private void OnEmplCommandExecuted(object o)
        {
            if (DateFrom > DateTo) {MessageBox.Show("Проверьте формат даты"); return; }
            if(SelectedEmployee==null) { MessageBox.Show("Выберите работника");return; }

            string sql = "select FirmName, UNP, Category.NameCategory, [Name],Price, Store.[Count], TotalPrice, PurchaseDay, [Description] " +
                "from Store " +
                "join Firms on(Store.FirmID = Firms.FirmID) " +
                "join Storage on(Store.StorageID= Storage.StorageID) " +
                "join Category on(Storage.CategoryID= Category.CategoryID) " +
                "where EmployeeID = (select EmployeeID from Employee where EmpLastName='" + SelectedEmployee.Split()[0] + "' and EmpFirstName = '" + SelectedEmployee.Split()[1] + "') " +
                "and PurchaseDay>= '" + DateFrom + "' and PurchaseDay<= '" + DateTo + "'";

            (new EmpReport()).writeClass(DateFrom, DateTo, "reportPeople","работнику", SelectedEmployee,sql, "Название", "УПН");
        }
        /// <summary>
        /// вывод отчета по работникам за данный период
        /// </summary>
        /// <param name="o"></param>
        private void OnEmplsCommandExecuted(object o)
        {
            if (DateFrom > DateTo) { MessageBox.Show("Проверьте формат даты"); return; }
        
            string sql = "select Employee.EmpFirstName, Employee.EmpLastName, sum(TotalPrice) as \"Продано на сумму\"" +
                "from Store " +
                "join Employee on (Store.EmployeeID = Employee.EmployeeID) " +
                "group by Employee.EmpFirstName, Employee.EmpLastName ";// +
             //   "PurchaseDay>= '" + DateFrom + "' and PurchaseDay<= '" + DateTo + "'";
            (new EmplsRep()).writeClass(DateFrom, DateTo, "reportPeople", "работникам", SelectedEmployee, sql, "Фамилия", "Имя");
        }

        /// <summary>
        /// вывод отчета по покупателю за данный период
        /// </summary>
        /// <param name="o"></param>
        private void OnCustCommandExecuted(object o)
        {
            if (DateFrom > DateTo) { MessageBox.Show("Проверьте формат даты"); return; }
            if (SelectedFirm == null) { MessageBox.Show("Выберите покупателя"); return; }

            string sql = " select EmpLastName, EmpFirstName, Category.NameCategory, [Name],Price, Store.[Count], TotalPrice, PurchaseDay, [Description] " +
                "from Store " +
                "join Employee on (Store.EmployeeID = Employee.EmployeeID) " +
                "join Storage on (Store.StorageID = Storage.StorageID) " +
                "join Category on (Storage.CategoryID = Category.CategoryID) " +
                "join Firms on (Store.FirmID = Firms.FirmID) " +
                "where Store.FirmID = (select FirmID from Firms where FirmName like '%" + SelectedFirm + "') " +
                "and PurchaseDay>= '" + DateFrom + "' and PurchaseDay<= '" + DateTo + "' ";


            (new EmpReport()).writeClass(DateFrom, DateTo, "reportPeople", "покупателю", SelectedFirm, sql, "Фамилия", "Имя");
        }
        /// <summary>
        /// очищает фильтры
        /// </summary>
        /// <param name="Select"></param>
        private void OnClearFilterCommandExecuted(object Select)
        {
            SelectNameItem = null;
            SelectNameCategoryItem = null;
            Text = null;
            OnPropertyChanged("SelectNameItem");
            OnPropertyChanged("SelectNameCategoryItem");
            OnPropertyChanged("Text");
            
           // FillListName();
            view.Filter = null;
        }
        private bool OnClearFilterCommandCanExecuted(object Select)
        {
            return !string.IsNullOrEmpty(SelectNameCategoryItem) || !string.IsNullOrEmpty(Text) || !string.IsNullOrEmpty(SelectNameItem);
        }
        private void Filter()
        {
            if (SelectNameCategoryItem == null && SelectNameItem == null && Text != null) { view.Filter = o => FilterTextBox(o); return; }
            if (SelectNameCategoryItem == null && SelectNameItem != null && Text == null) { view.Filter = o => FilterNameComboBox(o); return; }
            if (SelectNameCategoryItem != null && SelectNameItem == null && Text == null) { view.Filter = o => FilterNameCategoryComboBox(o); return; }
            if (SelectNameCategoryItem == null && SelectNameItem != null && Text != null) { view.Filter = o => FilterNameComboBox(o) && FilterTextBox(o); return; }
            if (SelectNameCategoryItem != null && SelectNameItem != null && Text == null) { view.Filter = o => FilterNameCategoryComboBox(o) && FilterNameComboBox(o); return; }
            if (SelectNameCategoryItem != null && SelectNameItem == null && Text != null) { view.Filter = o => FilterNameCategoryComboBox(o) && FilterTextBox(o); return; }
            if (SelectNameCategoryItem != null && SelectNameItem != null && Text != null) { view.Filter = o => FilterNameCategoryComboBox(o) && FilterNameComboBox(o) && FilterTextBox(o); return; }
           
        }
        private bool FilterNameCategoryComboBox(object o)
        {
            Storage c = o as Storage;
            return c.NameCategory.ToLowerInvariant().Contains(SelectNameCategoryItem.ToLower());
        }
        private bool FilterNameComboBox(object o)
        {
            Storage c = o as Storage;
            return c.Name.ToLowerInvariant().Contains(SelectNameItem.ToLower());
        }
        private bool FilterTextBox(object o)
        {
            Storage c = o as Storage;
            return c.Description.ToLower().Contains(Text.ToLower());
        }
        /// <summary>
        /// заполнение DataGrid названиями товаров
        /// </summary>
        private void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select Storage.StorageID, Category.NameCategory, Unit.UnitName, Storage.[Name], Storage.[Count], Storage.[Description], Storage.Price from Storage " +
                    "join Unit on (Storage.UnitID=Unit.UnitID) " +
                    "join Category on(Storage.CategoryID = Category.CategoryID)", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Storedb");

                if (storages == null)
                    storages = new ObservableCollection<Storage>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    storages.Add(new Storage
                    {
                        idStorage = Convert.ToInt32(dr[0].ToString()),
                        NameCategory = dr[1].ToString(),
                        UnitName = dr[2].ToString(),
                        Name = dr[3].ToString(),
                        Count = Convert.ToByte(dr[4].ToString()),
                        Description = (dr[5].ToString()),
                        Price = Convert.ToDouble(dr[6].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(storages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        /// <summary>
        /// заполнение comboBox названиями категорий
        /// </summary>
        private void FillListNameCategory()
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();

                using (com = new SqlCommand("select distinct NameCategory from Category", con))
                {
                    dt.Load(com.ExecuteReader());
                }
                con.Close();
            }
            if (namesCategory == null)
                namesCategory = new ObservableCollection<String>();
            foreach (DataRow dr in dt.Rows)
            {
                namesCategory.Add(dr[0].ToString());
            }
        }
        private void FillListNameWithCategory()
        {
            names.Clear();
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();

                using (com = new SqlCommand("select distinct [Name] from Storage " +
                    "join Category on (Storage.CategoryID=Category.CategoryID) " +
                    "where NameCategory like'%" + SelectNameCategoryItem + "'", con))
                {
                    dt.Load(com.ExecuteReader());
                }
                con.Close();
            }

            //    names = new List<String>();
            foreach (DataRow dr in dt.Rows)
            {
                names.Add(dr[0].ToString());
            }

        }
        /// <summary>
        /// заполнение comboBox названий товаров
        /// </summary>
        private void FillListName()
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();

                using (com = new SqlCommand("select distinct [Name] from Storage", con))
                {
                    dt.Load(com.ExecuteReader());
                }
                con.Close();
            }
            if (names == null)
                names = new ObservableCollection<string>();
            foreach (DataRow dr in dt.Rows)
            {
                names.Add(dr[0].ToString());
            }
        }
        /// <summary>
        /// заполнение comboBox работников
        /// </summary>
        private void FillListEmployee()
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();

                using (com = new SqlCommand("select distinct EmpLastName, EmpFirstName from Employee", con))
                {
                    dt.Load(com.ExecuteReader());
                }
                con.Close();
            }
            employees = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                employees.Add(dr[0].ToString() + " " + dr[1].ToString());
            }
        }
        /// <summary>
        /// заполнение comboBox покупателей
        /// </summary>
        private void FillListFirms()
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();
                using (com = new SqlCommand("select distinct FirmName from Firms", con))
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
}
