using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class AuthorizationSettings
    {
        public static string connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=storedb;Integrated Security=True";  
           // @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|storedb.mdf;Integrated Security=True;Connect Timeout=30;";

        public static int UserId;
        public static int EmployeeId;
        public static string EmpFirstName;
        public static string EmpLastName;
        public static string EmpPatronymic;
        public static string Access;
    }
}
