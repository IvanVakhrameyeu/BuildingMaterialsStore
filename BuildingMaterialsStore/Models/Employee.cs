using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Employee
    {
        public int idEmployee;
        public string EmpLastName { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpPatronymic { get; set; }
        public string Sex { get; set; }
        public DateTime EmpDateOfBirth { get; set; }
        public string EmpAddress { get; set; }
        public string EmpPhoneNumber { get; set; }
        public string Position { get; set; }
        public int Experience { get; set; }
    }
}
