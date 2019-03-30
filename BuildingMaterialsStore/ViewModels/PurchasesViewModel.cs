using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class PurchasesViewModel : ViewModel
    {
        static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=Storedb;Integrated Security=True";
        SqlConnection con;
        private SqlCommand com;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        public ObservableCollection<Purchases> purchases;
        public ICollectionView view { get; set; }
        private Purchases _selectItemDataGrid = null;
        public Purchases SelectItemDataGrid
        {
            get { return _selectItemDataGrid; }
            set
            {
                if (_selectItemDataGrid == value)
                    return;

                _selectItemDataGrid = value;
            }
        }
        public ICommand QuitAplicationCommand { get; }
        public ICommand FinishCommand { get; }
        public ICommand DeleteCommand { get; }
        private double _inTotal = 0;
        private List<Purchases> delList;
        public double InTotal
        {
            get { return _inTotal; }
            set { _inTotal = value; }
        }
        public PurchasesViewModel(List<Purchases> InputPurchases)
        {
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecuted);
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            FinishCommand = new DelegateCommand(AddInDB);
            if (InputPurchases.Count <= 0) return;
            FillListPurchases(InputPurchases);
            delList = InputPurchases;


        }
        private void AddInDB(object o) // вот тут я нажимаю
        {
            foreach (Purchases dr in delList)
            {
                  add(Purchases.idEmployee, dr.idCustomer, dr.idstorage,dr.Count,dr.Total);   // твой код
                //outPutdb("exec InputStore @EmployeeID="+Purchases.idEmployee.ToString() + ", " +   // мой старый код
                //    "@CustomerID="+ dr.idCustomer.ToString() + ", " +
                //    "@StorageID=" + dr.storage.idStorage.ToString() + ", " +
                //    "@Count=" + dr.Count.ToString() + ", " +
                //    "@TotalPrice=" + dr.Total.ToString() +"");
                MessageBox.Show(dr.idPurchases.ToString());
            }
            End();
        }
        public void outPutdb(string sql) // вывод из бд
        {
            using (con = new SqlConnection(connectionString))
            {
                con.Open();
                // Создаем объект DataAdapter (отправляет запрос на бд)
                adapter = new SqlDataAdapter(sql, con);
                // Создаем объект Dataset (представление о таблице)
               // DataSet ds = new DataSet();
                // Заполняем Dataset
               // adapter.Fill(ds);
               // return ds;
            }
        }
        private void add(int EmployeeID, int CustomerID, int StorageID, int Count, double TotalPrice)
        {            
            try
            {
                //string insert= "INSERT INTO Store(EmployeeID, CustomerID, StorageID, [Count], TotalPrice) " +
                //        "VALUES (@EmployeeID, @CustomerID, @StorageID, @[Count], @TotalPrice)";                            
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("InputStore", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                        com.Parameters.AddWithValue("@CustomerID", SqlDbType.Int).Value = CustomerID;
                        com.Parameters.AddWithValue("@StorageID", SqlDbType.Int).Value = StorageID;
                        com.Parameters.AddWithValue("@Count", SqlDbType.TinyInt).Value = Count;
                        com.Parameters.AddWithValue("@TotalPrice", SqlDbType.Float).Value = TotalPrice;
                        // DataTable dt = new DataTable();
                        com.ExecuteReader();
                        // i = (int)dt.Rows[0][0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
                //adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        private void End()
        {
            delList.RemoveRange(0,delList.Count);
            for(int i=0;i< purchases.Count;i++)
            purchases.RemoveAt(0);
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowCustomerPurchases")
                    item.Close();
            }
        }
        private void OnDeleteCommandExecuted(object o)
        {            
            delList.Remove(SelectItemDataGrid);
            purchases.Remove(SelectItemDataGrid);
            Total();
        }       
        private void Total()
        {
            InTotal = 0;
            foreach (Purchases dr in purchases)
            {                
                InTotal += dr.Total;
            }
            OnPropertyChanged("InTotal");
        }
        public void FillListPurchases(List<Purchases> InputPurchases)
        {
            try
            {
                if (purchases == null)
                    purchases = new ObservableCollection<Purchases>();

                foreach (Purchases dr in InputPurchases)
                {
                    purchases.Add(new Purchases
                    {
                        idPurchases = dr.idPurchases,
                        idstorage = dr.idstorage,
                        Count = dr.Count,
                        Total = dr.Total,
                        storage = dr.storage,

                    });
                    InTotal += dr.Total;
                }
                view = CollectionViewSource.GetDefaultView(purchases);
            }
            catch (Exception ex)
            {

            }
        }
        private void CloseExcute(object t)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowCustomerPurchases")
                    item.Close();
            }
        }
    }
}
