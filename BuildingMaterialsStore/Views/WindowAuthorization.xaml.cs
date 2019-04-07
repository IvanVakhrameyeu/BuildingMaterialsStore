using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuildingMaterialsStore.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowAuthorization.xaml
    /// </summary>
    public partial class WindowAuthorization : Window
    {
        public WindowAuthorization()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=storedb;Integrated Security=True";
            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds;

            con = new SqlConnection(connectionString);
            con.Open();
            cmd = new SqlCommand("select Users.UsersID, AccessName, [Login], [Password], EmployeeID, EmpFirstName, EmpLastName, EmpPatronymic " +
                "from Users " +
                "join Employee on (Employee.UsersID = Users.UsersID)" +
                "join Access on (Users.AccessID = Access.AccessID)" +
                "where[Login] = '"+ Login.Text + "' ", con);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "storedb");

            if (ds.Tables[0].DefaultView.Count < 1 || (Password.Password.ToString()!= ds.Tables[0].DefaultView[0].Row[3].ToString()) ||
                ds.Tables[0].DefaultView[0].Row[4]==null)
            { MessageBox.Show("Неправильный логин или пароль"); return; }
            AuthorizationSettings.UserId = Convert.ToInt32(ds.Tables[0].DefaultView[0].Row[0]);
            AuthorizationSettings.Access = ds.Tables[0].DefaultView[0].Row[1].ToString();
            
            AuthorizationSettings.EmployeeId = Convert.ToInt32(ds.Tables[0].DefaultView[0].Row[4]);
            AuthorizationSettings.EmpFirstName = ds.Tables[0].DefaultView[0].Row[5].ToString();
            AuthorizationSettings.EmpLastName = ds.Tables[0].DefaultView[0].Row[6].ToString();
            AuthorizationSettings.EmpPatronymic = ds.Tables[0].DefaultView[0].Row[7].ToString();


            Purchases.idEmployee = AuthorizationSettings.EmployeeId;
            ds = null;
            adapter.Dispose();
            con.Close();
            con.Dispose();

            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
