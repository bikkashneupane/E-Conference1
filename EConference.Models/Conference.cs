using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EConference.Models
{
    public class Conference
    {
        [Key]
        public int ID { get; set; }
        public virtual ConferenceName Name { get; set; }
        public virtual List<Papers> Papers { get; set; }
        public string ScheduledDate { get; set; }
    }
}
