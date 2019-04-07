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
    class EmployeeViewModel : ViewModel
    {
        static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=storedb;Integrated Security=True";
        SqlConnection con;
        private SqlCommand com;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private List<Purchases> purchases { get; set; } = null;
        private string _section;
        private string _selectItem = null;
        private string _selectCustomer = null;
        private string _text = null;
        public ICollectionView view { get; set; }
        public ObservableCollection<Employee> employee { get; set; }
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
                purchases = new List<Purchases>();
            }
        }
      
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
        public EmployeeViewModel(string section)
        {
            asyncMainMethod(section);
        }
        private void asyncMainMethod(string section)
        {
            CurrentSection = section;
            FillList();
        }     
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select * from Employee ", con);
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
                        EmpLastName= dr[1].ToString(),
                        EmpFirstName= (dr[2].ToString()),
                        EmpPatronymic= (dr[3].ToString()),
                        Sex = (dr[4].ToString()),
                        EmpDateOfBirth= Convert.ToDateTime(dr[5].ToString()),
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
