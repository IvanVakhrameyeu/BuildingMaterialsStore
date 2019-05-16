using BuildingMaterialsStore.Models;
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
using Wpf_журнал_учащихся_школы;

namespace BuildingMaterialsStore.ViewModels
{
    class PurchasesViewModel : ViewModel
    {
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
        private static double _inTotal = 0;
        private List<Purchases> delList;
        public static double InTotal
        {
            get { return _inTotal; }
            set { _inTotal = value; }
        }
        public PurchasesViewModel(List<Purchases> InputPurchases)
        {
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecuted);
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            FinishCommand = new DelegateCommand(AddInDB);
            try
            {
                if (InputPurchases.Count <= 0) return;
                FillListPurchases(InputPurchases);
                delList = InputPurchases;
                Total();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// вывод из бд необходимых данных
        /// </summary>
        /// <param name="o"></param>
        private void AddInDB(object o)
        {
            try
            {
                foreach (Purchases dr in delList)
                {
                    //try
                    //{
                        SqlConnection con;
                        SqlCommand cmd;
                        SqlDataAdapter adapter;
                        DataSet ds;
                        con = new SqlConnection(AuthorizationSettings.connectionString);
                        con.Open();
                        cmd = new SqlCommand("select FirmDiscountAmount from Firms where FirmID=" + dr.idFirm, con);
                        adapter = new SqlDataAdapter(cmd);
                        ds = new DataSet();
                        adapter.Fill(ds, "Storedb");

                        dr.CurrentDiscountAmount = Convert.ToDouble(ds.Tables[0].DefaultView[0].Row[0]);
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                    //finally
                    //{
                        con.Close();
                        con.Dispose();
                    //}
                    Add(Purchases.idEmployee, dr.idFirm, dr.idstorage, dr.Count);
                    
                }
               (new WorkWithWord()).writeClass(delList, "Invoice");
                InTotal = 0;                
                MainViewModel.isChange = true;
                End();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// добавление в бд покупки с уменьшением кол-ва товара
        /// </summary>
        /// <param name="EmployeeID">номер работника</param>
        /// <param name="CustomerID">номер покупателя</param>
        /// <param name="StorageID">номер товара</param>
        /// <param name="Count">количество товара</param>
        /// <param name="TotalPrice">общая цена</param>
        private void Add(int EmployeeID, int CustomerID, int StorageID, int Count)
        {
            try
            {
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("InputStore", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                        com.Parameters.AddWithValue("@FirmID", SqlDbType.Int).Value = CustomerID;
                        com.Parameters.AddWithValue("@StorageID", SqlDbType.Int).Value = StorageID;
                        com.Parameters.AddWithValue("@Count", SqlDbType.TinyInt).Value = Count;
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
        /// <summary>
        /// закрытие окна с отчисткой корзины
        /// </summary>
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
            foreach (Purchases dr in StorageViewModel.purchases)
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
