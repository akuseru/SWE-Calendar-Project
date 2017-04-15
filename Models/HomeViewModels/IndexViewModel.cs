using System;
using System.Collections.Generic;

namespace cal.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public DateTime Sunday { get; set; }
        public List<CalendarEvent> Events{get;set;}
    }
}
