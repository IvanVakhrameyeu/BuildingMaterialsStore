using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Store
    {
        public int StoreID { get; set; }
        public int EmployeeID { get; set; }
        public int FirmID { get; set; }
        public string FirmName { get; set; }
        public string Description { get; set; }
        public int StorageID { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public double CurrentDiscountAmount { get; set; }
        public DateTime PurchaseDay { get; set; }
        public bool Paid { get; set; }
    }
}
