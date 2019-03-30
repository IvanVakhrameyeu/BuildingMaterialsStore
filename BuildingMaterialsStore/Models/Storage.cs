using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    public class Storage
    {
        public int idStorage { get; set; }
        public string NameCategory { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public byte Count { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            Storage Storage1 = (Storage)obj;

            return (this.idStorage == Storage1.idStorage) && (this.NameCategory == Storage1.NameCategory)
                && (this.UnitName == Storage1.UnitName) && (this.Name == Storage1.Name)
                && (this.Count == Storage1.Count) && (this.Description==Storage1.Description)
                && (this.Price==Storage1.Price);
        }
    }
}
