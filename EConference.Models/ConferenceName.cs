using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EConference.Models
{
    public class ConferenceName
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(6)]
        [DisplayName("Conference Name")]
        public string name { get; set; }
    }
}
