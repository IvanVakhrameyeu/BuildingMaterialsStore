using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views.AddWindow;
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
    class GoodsVM
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        //private string _section;
        //private string _selectGoods = null;
        public ICollectionView view { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<Storage> storages { get; set; }

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
        public GoodsVM()
        {
            AddCommand = new DelegateCommand(OnAddCommandExecuted);
            FillList();
        }
        private void OnAddCommandExecuted(object o)
        {
            (new AddGoods(SelectItemDataGrid.idStorage)).ShowDialog();
            storages.Clear();
            FillList();
        }
        public void FillList()
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
    }
}
