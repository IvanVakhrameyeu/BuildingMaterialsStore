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

namespace BuildingMaterialsStore.ViewModels
{
    class FirmsViewModel
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        private string _section;
        private string _selectCustomer = null;
        public ICollectionView view { get; set; }
        public ICommand AddCommand { get; }
        public ObservableCollection<Firms> firms { get; set; }
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
        public FirmsViewModel()
        {
            AddCommand = new DelegateCommand(OnAddCommandExecuted);
            asyncMainMethod();
        }
        private void asyncMainMethod()
        {
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
        private void OnAddCommandExecuted(object o)
        {
            new WindowAddCustomer().ShowDialog();
            firms.Clear();
            FillList();

            MainViewModel.firms.Clear();
        }
        public void FillList()
        {
            try
            {
                con = new SqlConnection(AuthorizationSettings.connectionString);
                con.Open();
                cmd = new SqlCommand("select * " +
                    "from Firms ", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "storedb");

                if (firms == null)
                    firms = new ObservableCollection<Firms>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    firms.Add(new Firms
                    {
                        idFirm = Convert.ToInt32(dr[0].ToString()),
                        FirmName = dr[1].ToString(),
                        UNP = (dr[2].ToString()),
                        FirmLegalAddress = (dr[3].ToString()),
                        FirmAccountNumber = (dr[4].ToString()),
                        FirmBankDetails = (dr[5].ToString()),
                        FirmDiscountAmount = Convert.ToDouble(dr[6].ToString())
                    });
                }
                view = CollectionViewSource.GetDefaultView(firms);
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
