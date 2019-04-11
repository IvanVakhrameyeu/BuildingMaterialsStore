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
        public List<string> customers { get; set; }
        public List<string> namesCategory { get; set; }
        public List<string> names { get; set; }

        public ICollectionView view { get; set; }
        public ICommand PurchasesRepCommand { get; }
        public ICommand EmplRepCommand { get; }
        public ICommand CustRepCommand { get; }
        public ICommand ClearFilterCommand { get; }

        private string _selectNameItem = null;
        private string _selectNameCategoryItem = null;
        private string _text = null;

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
              //  FillListNameWithCategory();
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
            CustRepCommand = new DelegateCommand(OnCustCommandExecuted);
            CustRepCommand = new DelegateCommand(OnCustCommandExecuted);
            ClearFilterCommand = new DelegateCommand(OnClearFilterCommandExecuted, OnClearFilterCommandCanExecuted);
        }
        /// <summary>
        /// заполнение всех comboBox
        /// </summary>
        private void asyncMainMethod()
        {
            FillList();
            FillListNameCategory();
            FillListName();
            FillListEmployee();
            FillListCustomer();
        }
        /// <summary>
        /// вывод покупок за данный период(удалить возможно)
        /// </summary>
        /// <param name="o"></param>
        private void OnPurchasesCommandExecuted(object o)
        {

        }
        /// <summary>
        /// вывод отчета по работникам за данный период
        /// </summary>
        /// <param name="o"></param>
        private void OnEmplCommandExecuted(object o)
        {
            if (DateFrom > DateTo) {MessageBox.Show("Проверьте формат даты"); return; }
            EmpReport.writeClass(DateFrom, DateTo, "reportPeople",);
        }
        /// <summary>
        /// вывод отчета по покупателям за данный период
        /// </summary>
        /// <param name="o"></param>
        private void OnCustCommandExecuted(object o)
        {

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
            if (names == null)
                namesCategory = new List<String>();
            foreach (DataRow dr in dt.Rows)
            {
                namesCategory.Add(dr[0].ToString());
            }
        }
        private void FillListNameWithCategory()
        {
            MessageBox.Show("ololo");
            names.Clear();
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();

                using (com = new SqlCommand("select distinct [Name] from Storage " +
                    "join Category on (Storage.CategoryID=Category.CategoryID) " +
                    "where NameCategory= '" + SelectNameCategoryItem + "'", con))
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
                names = new List<String>();
            foreach (DataRow dr in dt.Rows)
            {
                names.Add(dr[0].ToString());
            }
        }
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
        private void FillListCustomer()
        {
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
}
