using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace EConference.Models
{
    public class Papers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PaperId { get; set; }

        [Required]
        public int PaperTopic { get; set; }

        [Required]
        public string PaperTitle { get; set; }

        [Required]
        [MaxLength(3)]
        public string Publisher { get; set; }

        [Required]
        public string Participant { get; set; }

        [Required]
        public string TimeZone { get; set; }

        [Required]
        public string Country { get; set; }

        public int? ConferenceID { get; set; }
        [ForeignKey("ConferenceID")]
        public virtual ConferenceName ConferenceName { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
