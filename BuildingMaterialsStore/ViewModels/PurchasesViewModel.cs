using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Wpf_журнал_учащихся_школы;

namespace BuildingMaterialsStore.ViewModels
{
    class PurchasesViewModel : ViewModel
    {
        static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=Storedb;Integrated Security=True";
        SqlConnection con;
        private SqlCommand com;
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
        private void AddInDB(object o)
        {
            foreach (Purchases dr in delList)
            {
                add(Purchases.idEmployee, dr.idCustomer, dr.idstorage, dr.Count, dr.Total);                 
            }
            WorkWithWord.writeClass(delList,"reportPurchases");
            End();
        }      
        private void add(int EmployeeID, int CustomerID, int StorageID, int Count, double TotalPrice)
        {
            try
            { 
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
                    com.Parameters.AddWithValue("@PurchaseDay", SqlDbType.Float).Value = DateTime.Now;
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        private void End()
        {
            delList.RemoveRange(0, delList.Count);
            for (int i = 0; i < purchases.Count; i++)
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
        private void FillListPurchases(List<Purchases> InputPurchases)
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
                MessageBox.Show(ex.Message);
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
