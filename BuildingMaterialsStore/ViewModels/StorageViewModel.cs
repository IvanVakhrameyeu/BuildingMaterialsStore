using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views;
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
    class StorageViewModel : ViewModel
    {
        SqlConnection con;
        private SqlCommand com;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private List<Purchases> purchases { get; set; } = null;
        private string _section;
        private string _selectItem = null;
        private string _selectCustomer = null;
        private string _text = null;
        public ICollectionView view { get; set; }
        public ICommand ClearFilterCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand ShoppingBasket { get; }
        public List<String> names { get; set; }
        public List<String> customers { get; set; }
        public ObservableCollection<Storage> storages { get; set; }
        public string CurrentSection
        {
            get { return _section; }
            set
            {
                if (_section == null)
                    _section = value;
            }
        }
        public string SelectItem
        {
            get { return _selectItem; }
            set
            {
                if (_selectItem == value) return;
                _selectItem = value;
                Filter();
            }
        }
        public string SelectCustomer
        {
            get { return _selectCustomer; }
            set
            {
                if (_selectCustomer == value) return;
                _selectCustomer = value;
                purchases = new List<Purchases>();
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
        public StorageViewModel(string section)
        {
            ClearFilterCommand = new DelegateCommand(OnClearFilterCommandExecuted, OnClearFilterCommandCanExecuted);
            AddCommand = new DelegateCommand(OnAddCommandExecuted);
            ShoppingBasket = new DelegateCommand(OnAddPurchaseCommandExecuted);
            asyncMainMethod(section);
        }
        private void OnAddCommandExecuted(object o)
        {
            Purchases pr = new Purchases();
            pr.storage = new Storage();
            pr.idstorage = SelectItemDataGrid.idStorage;
            
            if (purchases == null)
            {
                purchases = new List<Purchases>();
            }
            if (purchases.Count <= 0)
                pr.idPurchases = 1;
            else
                pr.idPurchases = purchases.Count + 1;
            pr.storage.idStorage = SelectItemDataGrid.idStorage;
            pr.idCustomer = findCustomer();
            if (pr.idCustomer == 0)
            {
                MessageBox.Show("Выберите покупателя");
                return;
            }
            pr.CustLastName = SelectCustomer.Split()[0];
            pr.CustFirstName = SelectCustomer.Split()[1];

            pr.storage.NameCategory = SelectItemDataGrid.NameCategory;
            pr.storage.Name = SelectItemDataGrid.Name;
            pr.storage.Price = SelectItemDataGrid.Price;
            pr.storage.UnitName = SelectItemDataGrid.UnitName;
            pr.storage.Description = SelectItemDataGrid.Description;

            new WindowAddPurchase(pr, SelectItemDataGrid.NameCategory, SelectItemDataGrid.Name, SelectItemDataGrid.Description, SelectItemDataGrid.Price).ShowDialog();
            if (pr != null && purchases != null)
            {
                if (!(pr.Count <= 0))
                    purchases.Add(pr);
            }
        }
        private int findCustomer()
        {
            int idCustomer = 0;
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select Customer.CustomerID from Customer " +
                    "where Customer.CustLastName like'%" + SelectCustomer.Split()[0] + "%' AND " +
                    "Customer.CustFirstName like '%" + SelectCustomer.Split()[1] + "%'", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Storedb");

                idCustomer = Convert.ToInt32(ds.Tables[0].DefaultView[0].Row[0]);
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
            return idCustomer;
        }
        private void OnAddPurchaseCommandExecuted(object o)
        {
            new WindowCustomerPurchases(purchases).ShowDialog();
        }
        async private void asyncMainMethod(string section)
        {
            CurrentSection = section;
            switch (section)
            {
                case "Главная": FillList(); break;
                default:
                    List(section);
                    break;
            }
            await Task.Run(() => FillListName());
            await Task.Run(() => FillListCustomer());
        }

        private void OnClearFilterCommandExecuted(object Select)
        {
            SelectItem = null;
            Text = null;
            OnPropertyChanged("SelectItem");
            OnPropertyChanged("Text");
            view.Filter = null;
        }
        private bool OnClearFilterCommandCanExecuted(object Select)
        {
            return !string.IsNullOrEmpty(SelectItem) || !string.IsNullOrEmpty(Text);
        }
        private bool FilterComboBox(object o)
        {
            Storage c = o as Storage;
            return c.Name.ToLowerInvariant().Contains(SelectItem.ToLower());
        }
        private bool FilterTextBox(object o)
        {
            Storage c = o as Storage;
            return c.Description.ToLower().Contains(Text.ToLower());
        }
        private void Filter()
        {
            if (SelectItem != null && Text == null) { view.Filter = o => FilterComboBox(o); return; }
            if (Text != null && SelectItem == null) { view.Filter = o => FilterTextBox(o); return; }
            if (Text != null && SelectItem != null) { view.Filter = o => FilterComboBox(o) && FilterTextBox(o); return; }
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
        private void List(string Category)
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select Storage.StorageID, Category.NameCategory, Unit.UnitName, Storage.[Name], Storage.[Count], Storage.[Description], Storage.Price from Storage " +
                    "join Unit on (Storage.UnitID=Unit.UnitID) " +
                    "join Category on(Storage.CategoryID = Category.CategoryID)" +
                    "where Category.NameCategory='" + Category + "'", con);
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
        private void FillListName()
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(AuthorizationSettings.connectionString))
            {
                con.Open();
                if (CurrentSection != "Главная")
                {
                    using (com = new SqlCommand("select distinct [Name] from Storage  where CategoryID = (select CategoryID from Category where NameCategory = '" + CurrentSection + "')", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
                }
                else
                {
                    using (com = new SqlCommand("select distinct [Name] from Storage", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
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

            if (customers == null)
                customers = new List<String>();

            foreach (DataRow dr in dt.Rows)
            {
                customers.Add(dr[0].ToString() + " " + dr[1].ToString());
            }
        }
    }
}
