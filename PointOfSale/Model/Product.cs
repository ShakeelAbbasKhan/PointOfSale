namespace PointOfSale.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
