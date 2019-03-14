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

namespace BuildingMaterialsStore.ViewModels
{
    class StorageViewModel : INotifyPropertyChanged
    {   
            static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=Storedb;Integrated Security=True";
            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds;
            public ObservableCollection<Storage> storages { get; set; }
            public StorageViewModel(string section)
            {
                switch (section)
                {
                    case "Главная": FillList(); break;
                    //case "Клиент": break;
                default: List(section); break;
                }
            }

            public void FillList()
            {
                try
                {
                    con = new SqlConnection(connectionString);
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
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    ds = null;
                    adapter.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            public void List(string Category)
            {
                try
                {
                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("select Storage.StorageID, Category.NameCategory, Unit.UnitName, Storage.[Name], Storage.[Count], Storage.[Description], Storage.Price from Storage " +
                        "join Unit on (Storage.UnitID=Unit.UnitID) " +
                        "join Category on(Storage.CategoryID = Category.CategoryID)" +
                        "where Category.NameCategory='"+ Category +"'", con);
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
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    ds = null;
                    adapter.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            #endregion
     
}
}
