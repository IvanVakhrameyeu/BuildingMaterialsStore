using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Storage
    {
        public int idProduct;
        public string Name;
        public uint Count;
        public string Description;
        public float Price;

        public int? idCategory;
        public Category Category;
    }
}
