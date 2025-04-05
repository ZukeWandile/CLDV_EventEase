using System.ComponentModel.DataAnnotations;

namespace WebApplication_EVENTEASE_ST10448895.Models
{
    public class Venue
    {
        [Key]
        public int Venue_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Venue_Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Locations { get; set; }
        [Required]
        public int Capacity { get; set; }

        public byte[]? ImageUrl { get; set; }
    }
}
