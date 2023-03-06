
namespace DenaAPI.Models
{
    public partial class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CatId { get; set; }

        public virtual ICollection<Intermediate> Intermediates { get; } = new List<Intermediate>();
    }
}
