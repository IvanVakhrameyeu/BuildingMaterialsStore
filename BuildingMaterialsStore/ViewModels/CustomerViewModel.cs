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

namespace BuildingMaterialsStore.ViewModels
{
    class CustomerViewModel
    {
        static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=storedb;Integrated Security=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private string _section;
        private string _selectCustomer = null;
        public ICollectionView view { get; set; }
        //public ICommand DelCommand { get; }
        public ObservableCollection<Customer> customer { get; set; }
        public string CurrentSection
        {
            get { return _section; }
            set
            {
                if (_section == null)
                    _section = value;
            }
        }
        public string SelectCustomer
        {
            get { return _selectCustomer; }
            set
            {
                if (_selectCustomer == value) return;
                _selectCustomer = value;
            }
        }

        private Employee _selectItemDataGrid = null;
        public Employee SelectItemDataGrid
        {
            get { return _selectItemDataGrid; }
            set
            {
                if (_selectItemDataGrid == value)
                    return;

                _selectItemDataGrid = value;
            }
        }
        public CustomerViewModel(string section)
        {
         //   DelCommand = new DelegateCommand(OnDeleteCommandExecuted);
            asyncMainMethod(section);
        }
        private void asyncMainMethod(string section)
        {
            CurrentSection = section;
            FillList();
        }
        //private void OnDeleteCommandExecuted(object o)
        //{
        //    try
        //    {
        //        con = new SqlConnection(connectionString);
        //        con.Open();
        //        cmd = new SqlCommand("update Employee " +
        //            "set UsersID = null " +
        //            "where EmployeeID = " + SelectItemDataGrid.idEmployee, con);
        //        adapter = new SqlDataAdapter(cmd);
        //        ds = new DataSet();
        //        adapter.Fill(ds, "storedb");
        //        employee.Remove(SelectItemDataGrid);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        ds = null;
        //        adapter.Dispose();
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select CustomerID, CustLastName, CustFirstName, CustPatronymic, Sex, CustDateOfBirth, CustAddress, " +
                    "CustPhoneNumber " +
                    "from Customer ", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (customer == null)
                    customer = new ObservableCollection<Customer>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    customer.Add(new Customer
                    {
                        idCustomer = Convert.ToInt32(dr[0].ToString()),
                        CustLastName = dr[1].ToString(),
                        CustFirstName = (dr[2].ToString()),
                        CustPatronymic = (dr[3].ToString()),
                        Sex = (dr[4].ToString()),
                        CustDateOfBirth = Convert.ToDateTime(dr[5].ToString()),
                        CustAddress = (dr[6].ToString()),
                        CustPhoneNumber = (dr[7].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(customer);
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
