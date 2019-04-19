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

namespace BuildingMaterialsStore.ViewModels.Pages
{
    class GoodsIssueVM
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private string _section;
        private string _selectGoods = null;
        public ICollectionView view { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<Store> stories { get; set; }
        
        private Store _selectItemDataGrid = null;
        public Store SelectItemDataGrid
        {
            get { return _selectItemDataGrid; }
            set
            {
                if (_selectItemDataGrid == value)
                    return;

                _selectItemDataGrid = value;
            }
        }

        public GoodsIssueVM()
        {
            AddCommand = new DelegateCommand(OnAddCommandExecuted);
            FillList();
        }
        private void OnAddCommandExecuted(object o)
        {
            if (SelectItemDataGrid.Paid==true)
            {
                MessageBox.Show("уже отгружено");
                return;
            }


        }
        private void updatePurhcases()
        {

        }
        public void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select Store.StoreID, Store.EmployeeID, Store.FirmID, Firms.FirmName, Store.StorageID, " +
                    "Store.[Count], Store.TotalPrice, Store.CurrentDiscountAmount, " +
                    "Store.PurchaseDay, Store.Paid, Storage.[Description] " +
                    "from Store " +
                    "join Storage on (Store.StorageID= Storage.StorageID)  " +
                    "join Firms on (Store.FirmID= Firms.FirmID)", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (stories == null)
                    stories = new ObservableCollection<Store>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    stories.Add(new Store
                    {
                        StoreID = Convert.ToInt32(dr[0].ToString()),
                        //EmployeeID = Convert.ToInt32(dr[1].ToString()),
                        //FirmID = Convert.ToInt32(dr[2].ToString()),
                        FirmName = (dr[3].ToString()),
                        StorageID = Convert.ToInt32(dr[4].ToString()),
                        Count = Convert.ToInt32(dr[5].ToString()),
                        TotalPrice = Convert.ToDouble(dr[6].ToString()),
                        CurrentDiscountAmount = Convert.ToDouble(dr[7].ToString()),
                        //PurchaseDay = Convert.ToDateTime(dr[8].ToString()),
                        Paid = Convert.ToBoolean(dr[9].ToString()),
                        Description = (dr[10].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(stories);
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
    }
}
