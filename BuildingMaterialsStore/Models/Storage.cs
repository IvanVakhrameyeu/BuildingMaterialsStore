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
    }
}
