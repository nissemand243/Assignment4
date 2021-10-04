using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Key]
        [StringLength(50)]
        public string name { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
