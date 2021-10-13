using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [Key]
        [StringLength(100)]
        public string Title { get; set; }

        public User? AssignedTo { get; set; }

        public string? Description { get; set; }

        [Required]
        public State State{ get; set;}
        public ICollection<Tag> Tags { get; set; }

        public DateTime Created {get; init; }
        public DateTime StateUpdated {get; set;}

    }
}
