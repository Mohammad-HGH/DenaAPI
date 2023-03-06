namespace DenaAPI.Models
{
    public partial class Attribute
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public string? Brand { get; set; }

        public virtual ICollection<Intermediate> Intermediates { get; } = new List<Intermediate>();
    }
}
