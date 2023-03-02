namespace DenaAPI.Entities
{
    public partial class Intermediate
    {
        public Intermediate()
        {
            Products = new HashSet<Product>();
            Attributes = new HashSet<Attribute>();
        }
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; }
    }
}
