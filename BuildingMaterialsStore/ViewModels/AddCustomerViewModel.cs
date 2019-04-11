using BuildingMaterialsStore.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class AddCustomerViewModel : ViewModel
    {
        SqlConnection con;
        private SqlCommand com;

        private string _custLastName;
        private string _custFirstName;
        private string _custPatronymic;
        private string _sex;
        private DateTime _custDateOfBirth;
        private string _custAddress;
        private string _custPhoneNumber;

        public string CustLastName
        {
            get
            {
                return _custLastName;
            }
            set
            {
                _custLastName = value;
            }
        }
        public string CustFirstName
        {
            get
            {
                return _custFirstName;
            }
            set
            {
                _custFirstName = value;
            }
        }
        public string CustPatronymic
        {
            get
            {
                return _custPatronymic;
            }
            set
            {
                _custPatronymic = value;
            }
        }
        public string Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
            }
        }
        public DateTime CustDateOfBirth { get; set; }
        public string CustAddress
        {
            get
            {
                return _custAddress;
            }
            set
            {
                _custAddress = value;
            }
        }
        public string CustPhoneNumber
        {
            get
            {
                return _custPhoneNumber;
            }
            set
            {
                _custPhoneNumber = value;
            }
        }

        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }
        /// <summary>
        /// привязка делегатов в конструкторе
        /// </summary>
        public AddCustomerViewModel()
        {
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);
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
                    using (com = new SqlCommand("InsertCustomer", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@CustLastName", SqlDbType.VarChar).Value = CustLastName;
                        com.Parameters.AddWithValue("@CustFirstName", SqlDbType.VarChar).Value = CustFirstName;
                        com.Parameters.AddWithValue("@CustPatronymic", SqlDbType.VarChar).Value = CustPatronymic;
                        com.Parameters.AddWithValue("@Sex", SqlDbType.VarChar).Value = Sex;
                        com.Parameters.AddWithValue("@CustDateOfBirth", SqlDbType.Date).Value = CustDateOfBirth;
                        com.Parameters.AddWithValue("@CustAddress", SqlDbType.VarChar).Value = CustAddress;
                        com.Parameters.AddWithValue("@CustPhoneNumber", SqlDbType.VarChar).Value = CustPhoneNumber;
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
        /// добаление клиента и закрытие окна
        /// </summary>
        /// <param name="t"></param>
        private void Add(object t)
        {
            //AddInDB();
            //MessageBox.Show(CustDateOfBirth.ToString());
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
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddCustomer")
                    item.Close();
            }
        }
    }
}
