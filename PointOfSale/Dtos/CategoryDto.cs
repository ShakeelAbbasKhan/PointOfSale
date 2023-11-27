using PointOfSale.Model;

namespace PointOfSale.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategory> SubCategory { get; set; }
    }
}
