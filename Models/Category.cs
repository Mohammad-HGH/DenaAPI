using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
