using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Category
    {
        public int idCategory;
        public string NameCategory;

        public ICollection<Storage> Storages;

        public Category()
        {
            Storages = new List<Storage>();
        }
    }
}
