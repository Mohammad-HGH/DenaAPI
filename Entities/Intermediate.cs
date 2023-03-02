namespace DenaAPI.Entities
{
    public partial class Intermediate
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; }
    }
}
