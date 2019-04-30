using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.ViewModels.WordReports;
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
        //private string _section;
        //private string _selectGoods = null;
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

            int id = SelectItemDataGrid.FirmID;
            DateTime date = SelectItemDataGrid.PurchaseDay;
            try
            {                
                SqlCommand com;

                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("Shipment", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ID", SqlDbType.Int).Value = SelectItemDataGrid.FirmID;
                        com.Parameters.AddWithValue("@Date", SqlDbType.Date).Value = SelectItemDataGrid.PurchaseDay;
                        com.ExecuteNonQuery();
                    }
                    con.Close();


                    stories.Clear();
                
                    FillList();
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
            string sql = "select Storage.[Description], UnitName, Store.[Count], Store.TotalPrice, Storage.Price, Store.FirmID, Store.PurchaseDay " +
                "from Store " +
                "join Storage on(Store.StorageID = Storage.StorageID) " +
                "join Unit on(Storage.UnitID = Unit.UnitID) " +
                "where Store.PurchaseDay = '" + date + "' and " +
                "Store.FirmID = " + id + "";
            TTH.writeClass("TTH", sql);
        }
        public void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select Store.FirmID, Firms.FirmName, PurchaseDay, Store.Paid,  Sum(TotalPrice) as 'TotalPrice' from Store " +
                    "join Firms on(Store.FirmID = Firms.FirmID) " +
                    "group by Store.FirmID, PurchaseDay, Firms.FirmName, Store.Paid", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (stories == null)
                    stories = new ObservableCollection<Store>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    stories.Add(new Store
                    {                        
                        FirmID = Convert.ToInt32(dr[0]),
                        FirmName = dr[1].ToString(),
                        PurchaseDay = (Convert.ToDateTime(dr[2])),
                        TotalPrice = Convert.ToDouble(dr[4]),
                        Paid = Convert.ToBoolean(dr[3])
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
