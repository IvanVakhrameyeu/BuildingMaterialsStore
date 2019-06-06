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
        private string _firmName;
        private string _UNP;
        private string _firmAccountNumber;
        private string _firmBankDetails;
        private string _firmLegalAddress;
        private string _firmPhoneNumber;

        public string FirmName
        {
            get
            {
                return _firmName;
            }
            set
            {
                _firmName = value;
            }
        }
        public string UNP
        {
            get
            {
                return _UNP;
            }
            set
            {
                _UNP = value;
            }
        }
        public string FirmAccountNumber
        {
            get
            {
                return _firmAccountNumber;
            }
            set
            {
                _firmAccountNumber = value;
            }
        }
        public string FirmBankDetails
        {
            get
            {
                return _firmBankDetails;
            }
            set
            {
                _firmBankDetails = value;
            }
        }
        public string FirmLegalAddress
        {
            get
            {
                return _firmLegalAddress;
            }
            set
            {
                _firmLegalAddress = value;
            }
        }
        public string FirmPhoneNumber
        {
            get
            {
                return _firmPhoneNumber;
            }
            set
            {
                _firmPhoneNumber = value;
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
                    using (com = new SqlCommand("InsertFirm", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@FirmName", SqlDbType.VarChar).Value = FirmName;
                        com.Parameters.AddWithValue("@UNP", SqlDbType.VarChar).Value = UNP;
                        com.Parameters.AddWithValue("@FirmAccountNumber", SqlDbType.VarChar).Value = FirmAccountNumber;
                        com.Parameters.AddWithValue("@FirmBankDetails", SqlDbType.VarChar).Value = FirmBankDetails;
                        com.Parameters.AddWithValue("@FirmLegalAddress", SqlDbType.VarChar).Value = FirmLegalAddress;
                        com.Parameters.AddWithValue("@FirmPhoneNumber", SqlDbType.VarChar).Value = FirmPhoneNumber;
                        com.ExecuteNonQuery();
                    }
                    con.Close();
                    
                    if(AuthorizationSettings.Access=="Middle")
                    MainViewModel.firms.Add(FirmName); // ДОБАВЛЕНИЕ ФИРМЫ В КОМБОБОКС         

                  
                   else ReportViewModel.firms.Add(FirmName);
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
            AddInDB();
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
