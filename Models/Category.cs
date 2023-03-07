using System.ComponentModel.DataAnnotations;

namespace DenaAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public int? ParentId { get; set; }
    }
}
