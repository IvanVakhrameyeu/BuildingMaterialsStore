using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Customer
    {
        public int idCustomer;
        public string CustLastName { get; set; }
        public string CustFirstName { get; set; }
        public string CustPatronymic { get; set; }
        public string Sex { get; set; }
        public DateTime CustDateOfBirth { get; set; }
        public string CustAddress { get; set; }
        public double CustDiscountAmount { get; set; }
        public string CustPhoneNumber { get; set; }
    }
}
