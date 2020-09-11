using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EConference.Models
{
    public class RegisterPaper
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PaperId { get; set; }
        [Required]
        public string PaperTopic { get; set; }
        [Required]
        public string PaperTitle { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters Only")]
        [MaxLength(3)]
        public string Publisher { get; set; }
        [Required]
        public string Participant { get; set; }

        [NotMapped]
        public ConferenceName ConferenceStatus { get; set; }
    }
}
