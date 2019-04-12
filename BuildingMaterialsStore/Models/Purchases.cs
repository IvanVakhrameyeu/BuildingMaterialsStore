using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    public class Purchases
    {
        public static int idEmployee;
        public int idPurchases { get; set; }
        public int idstorage { get; set; }
        public int idCustomer { get; set; }
        public string CustFirstName { get; set; }
        public string CustLastName { get; set; }
        public int Count { get; set; }
        public double CurrentDiscountAmount { get; set; }
        public double Total { get; set; }

        public Storage storage { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            Purchases Purchases1 = (Purchases)obj;

            return (this.idCustomer == Purchases1.idCustomer) && (this.idPurchases == Purchases1.idPurchases) 
                && (this.idstorage==Purchases1.idstorage) && (this.Total==Purchases1.Total)
                && (this.storage.Equals(Purchases1.storage));
        }

        public override int GetHashCode()
        {
            var hashCode = -746117808;
            hashCode = hashCode * -1521134295 + idPurchases.GetHashCode();
            hashCode = hashCode * -1521134295 + idstorage.GetHashCode();
            hashCode = hashCode * -1521134295 + idCustomer.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + Total.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Storage>.Default.GetHashCode(storage);
            return hashCode;
        }
    }
}
