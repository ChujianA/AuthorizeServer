using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class Permissions
    {
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string ActionUrl { get; set; }
    }
}
