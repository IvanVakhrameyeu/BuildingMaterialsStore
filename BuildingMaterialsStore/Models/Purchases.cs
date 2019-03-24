using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    public class Purchases
    {
        public Storage storage { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }
    }
}
