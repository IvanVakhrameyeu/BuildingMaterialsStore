using BuildingMaterialsStore.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels.AddWindow
{
    class GoodsW : ViewModel
    {
        private int _countPurchases = 0;
        private int _id = 0;
        public int id
        {
            get { return _id; }
            set {_id = value; }
        }
        public int CountPurchases
        {
            get { return _countPurchases; }
            set
            {
                _countPurchases = value;
            }
        }
        public ICollectionView view { get; set; }
        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }
        public GoodsW(int id)
        {
            CountPurchases = 1;
            _id = id;
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);
        }
        /// <summary>
        /// добавление количества товаров на складе с закрытием окна
        /// </summary>
        /// <param name="t"></param>
        private void Add(object t)
        {
            if (CountPurchases < 1)
            {
                MessageBox.Show("Нельзя добавить меньше 1 товара"); return;
            }
            SqlConnection con;
            SqlCommand com;
            try
            {
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("AddGoods", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ID", SqlDbType.Int).Value = id;
                        com.Parameters.AddWithValue("@Count", SqlDbType.Date).Value = CountPurchases;
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
                //con.Close();
                //con.Dispose();
            }

            CloseExcute(new object());
        }
        /// <summary>
        /// закрытие окна
        /// </summary>
        /// <param name="t">любой объект</param>
        private void CloseExcute(object t)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.AddWindow.AddGoods")
                    item.Close();
            }
        }
    }
}
