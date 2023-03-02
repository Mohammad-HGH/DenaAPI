namespace DenaAPI.Entities
{
    public partial class Attribute
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int IntermediateId { get; set; }
        public virtual Intermediate Intermediate { get; set; }
    }
}
