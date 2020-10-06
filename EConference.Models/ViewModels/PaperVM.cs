using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EConference.Models.ViewModels
{
    public class PaperVM
    {
        public Papers Papers { get; set; }
        public IEnumerable<SelectListItem> ConferenceList { get; set; }
        public IEnumerable<SelectListItem> TopicList { get; set; }
    }
}
