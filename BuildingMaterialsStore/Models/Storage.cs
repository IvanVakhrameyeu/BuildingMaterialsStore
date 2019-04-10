using System.Collections.Generic;

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

            Storage storage = (Storage)obj;
            return (this.NameCategory == storage.NameCategory) && (this.UnitName == storage.UnitName) && (this.Name == storage.Name) && 
                (this.Count == storage.Count) && (this.Description == storage.Description) && (this.Price == storage.Price);
        }

        public override int GetHashCode()
        {
            var hashCode = 1547874760;
            hashCode = hashCode * -1521134295 + idStorage.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NameCategory);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UnitName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            return hashCode;
        }
    }
}
