namespace DenaAPI.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int IntermediateId { get; set; }
        public virtual Intermediate Intermediate { get; set; }
    }
}
