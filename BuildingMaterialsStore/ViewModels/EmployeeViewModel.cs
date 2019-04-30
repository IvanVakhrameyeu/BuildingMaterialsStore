using BuildingMaterialsStore.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class EmployeeViewModel : ViewModel
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private string _selectCustomer = null;
        public ICollectionView view { get; set; }
        public ICommand DelCommand { get; }
        public ObservableCollection<Employee> employee { get; set; }
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
        public EmployeeViewModel()
        {
            DelCommand = new DelegateCommand(OnDeleteCommandExecuted);
            asyncMainMethod();
        }
        private void asyncMainMethod()
        {
            FillList();
        }
        private void OnDeleteCommandExecuted(object o)
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("update Employee " +
                    "set UsersID = null " +
                    "where EmployeeID = "+ SelectItemDataGrid.idEmployee, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");
                employee.Remove(SelectItemDataGrid);
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
        public void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select EmployeeID, EmpLastName, EmpFirstName, EmpPatronymic, Sex, EmpDateOfBirth, EmpAddress, " +
                    "EmpPhoneNumber, Position, Experience " +
                    "from Employee " +
                    "where UsersID is not null", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (employee == null)
                    employee = new ObservableCollection<Employee>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    employee.Add(new Employee
                    {
                        idEmployee = Convert.ToInt32(dr[0].ToString()),
                        EmpLastName = dr[1].ToString(),
                        EmpFirstName = (dr[2].ToString()),
                        EmpPatronymic = (dr[3].ToString()),
                        Sex = (dr[4].ToString()),
                        EmpDateOfBirth = Convert.ToDateTime(dr[5].ToString()),
                        EmpAddress = (dr[6].ToString()),
                        EmpPhoneNumber = (dr[7].ToString()),
                        Position = dr[8].ToString(),
                        Experience = Convert.ToInt32(dr[9].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(employee);
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
