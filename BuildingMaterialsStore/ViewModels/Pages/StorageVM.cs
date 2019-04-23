using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels.Pages
{
    class StorageVM
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        public ICollectionView view { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<Storage> storagies { get; set; }

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
        public StorageVM()
        {
            AddCommand = new DelegateCommand(OnAddCommandExecuted);
            FillList();
        }
        private void OnAddCommandExecuted(object o)
        {
            new WindowAddStorage().ShowDialog();
            
        }

        public void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select StorageID, NameCategory, UnitName, [Name], [Count], [Description], Price  " +
                    "from Storage " +
                    "join Unit on (Storage.UnitID= Unit.UnitID)  " +
                    "join Category on (Storage.CategoryID= Category.CategoryID)" +
                    "", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (storagies == null)
                    storagies = new ObservableCollection<Storage>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    storagies.Add(new Storage
                    {
                        idStorage = Convert.ToInt32(dr[0].ToString()),                       
                        NameCategory = (dr[1].ToString()),
                        UnitName = (dr[2].ToString()),
                        Name  = (dr[3].ToString()),
                        Count = Convert.ToByte(dr[4].ToString()),
                        Description = (dr[5].ToString()),
                        Price = Convert.ToDouble(dr[6].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(storagies);
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
