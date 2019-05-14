using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class AddStorageVM : ViewModel
    {
        SqlConnection con;
        SqlCommand com;
        private int _categoryID;
        private int _unitID;
        private string _name;
        private byte _count;
        private string _description;
        private double _price;

        static public bool isChange = false;

        public List<string> NameCategory { get; set; }
        public List<string> UnitName { get; set; }
        public string Name { get { return _name; } set { _name = value; } }
        public byte Count { get { return _count; } set { _count = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public double Price { get { return _price; } set { _price = value; } }


        private string _selectUnitName = null;
        private string _selectCategory = null;
        public string SelectCategory
        {
            get { return _selectCategory; }
            set
            {
                if (_selectCategory == value) return;
                _selectCategory = value;
            }
        }
        public string SelectUnitName
        {
            get { return _selectUnitName; }
            set
            {
                if (_selectUnitName == value) return;
                _selectUnitName = value;
            }
        }
        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }
        /// <summary>
        /// привязка делегатов в конструкторе
        /// </summary>
        public AddStorageVM()
        {
            asyncMethod();
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);
        }
        private async void asyncMethod()
        {
            //await Task.Run(() => FullCategory());
            //await Task.Run(() => FullUnit());
            FullCategory();
            FullUnit();
        }
        private void FullUnit()
        {
            if (NameCategory == null)
            {
                SqlConnection con;
                SqlCommand com;
                DataTable dt = new DataTable();
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("select distinct CategoryID, NameCategory from Category", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
                    con.Close();
                }
                NameCategory = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    NameCategory.Add(dr[0].ToString()+" "+ dr[1].ToString());
                    //NameCategory.Add(new IdAndName
                    //{
                    //    id = Convert.ToInt32(dr[0]),
                    //    Name = dr[1].ToString()
                    //});
                }
            }
        }
        private void FullCategory()
        {
            if (UnitName == null)
            {
                SqlConnection con;
                SqlCommand com;
                DataTable dt = new DataTable();
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("select distinct UnitID, UnitName from Unit", con))
                    {
                        dt.Load(com.ExecuteReader());
                    }
                    con.Close();
                }
                UnitName = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    //MessageBox.Show(dr[0].ToString());
                    UnitName.Add(dr[0].ToString()+" " +dr[1].ToString());
                    //UnitName.Add(new IdAndName
                    //{
                    //    id = Convert.ToInt32(dr[0]),
                    //    Name = dr[1].ToString()
                    //});
                }
            }
        }
        /// <summary>
        /// добавление в базу данных нового клиента
        /// </summary>
        private void AddInDB()
        {
            try
            {
                using (con = new SqlConnection(AuthorizationSettings.connectionString))
                {
                    con.Open();
                    using (com = new SqlCommand("InsertStoragesName", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@CategoryID", SqlDbType.Int).Value = SelectCategory.Split(' ')[0];
                        com.Parameters.AddWithValue("@UnitID", SqlDbType.Int).Value = SelectUnitName.Split(' ')[0];
                        com.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                        com.Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = Description;
                        com.Parameters.AddWithValue("@Price", SqlDbType.Float).Value = Price;
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
                isChange = true;
            }
        }
        /// <summary>
        /// добаление клиента и закрытие окна
        /// </summary>
        /// <param name="t"></param>
        private void Add(object t)
        {
            try
            {
                if (Price<=0) { MessageBox.Show("Нельзя поставить такую цену");return; }
                AddInDB();
                CloseExcute(new object());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// закрытие окна
        /// </summary>
        /// <param name="t">любой объект</param>
        private void CloseExcute(object t)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddStorage")
                    item.Close();
            }
        }

    }
}
