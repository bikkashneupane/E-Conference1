using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.Models.ViewModels
{
    public class SchedulingViewModel
    {
        public List<List<Papers>> Groups { get; set; }
        public List<ConferenceName> Names { get; set; }
    }
}
