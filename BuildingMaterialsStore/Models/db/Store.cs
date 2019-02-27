using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Store
    {
        public int idStorage;
        public byte Count;
        public float Price;
        public double TotalPrice;

        public int? idCustomer;
        public Customer Customer;

        public int? idEmployee;
        public Employee Employee;
    }
}
