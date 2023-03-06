namespace DenaAPI.Models
{
    public partial class Intermediate
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int AttributeId { get; set; }

        public long Price { get; set; }

        public virtual Attribute Attribute { get; set; }

        public virtual Product Product { get; set; }
    }
}
