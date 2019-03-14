using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Storage
    {
        public int idStorage { get; set; }
        public string NameCategory { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public byte Count { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
