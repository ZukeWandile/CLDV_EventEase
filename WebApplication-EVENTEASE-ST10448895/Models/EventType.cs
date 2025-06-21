using System.ComponentModel.DataAnnotations;

namespace WebApplication_EVENTEASE_ST10448895.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation Property
        public ICollection<EventS>? Events { get; set; }
    }
}