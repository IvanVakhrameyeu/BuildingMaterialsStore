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
        public string CustLastName;
        public string CustFirstName;
        public string CustPatronymic;
        public DateTime CustDateOfBirth;
        public string CustAddress;
        public string CustPhoneNomber;

        ICollection<Storage> Storages;
        public Customer()
        {
            Storages = new List<Storage>();
        }
    }
}
