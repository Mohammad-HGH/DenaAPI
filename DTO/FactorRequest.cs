using System.ComponentModel.DataAnnotations;

namespace DenaAPI.DTO
{
    public class FactorRequest
    {
        public int? ProductId { get; set; }
        public int? Number { get; set; }
        public int? UserId { get; set; }
        public int? AttId { get; set; }

        public bool Payed { get; set; }

        public bool Accepted { get; set; }

        public bool Lapsed { get; set; }
    }
}
