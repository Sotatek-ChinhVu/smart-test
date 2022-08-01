using Domain.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class WeekOfMonthModel
    {
        public DayInfo Sun { get; set; }
        public DayInfo Mon { get; set; }
        public DayInfo Tue { get; set; }
        public DayInfo Wed { get; set; }
        public DayInfo Thu { get; set; }
        public DayInfo Fri { get; set; }
        public DayInfo Sat { get; set; }
        public WeekOfMonthModel(DayInfo sun, DayInfo mon, DayInfo tue, DayInfo wed, DayInfo thu, DayInfo fri, DayInfo sat)
        {
            Sun = sun ?? new();
            Mon = mon ?? new();
            Tue = tue ?? new();
            Wed = wed ?? new();
            Thu = thu ?? new();
            Fri = fri ?? new();
            Sat = sat ?? new();
        }
        public WeekOfMonthModel()
        {
            Sun = new();
            Mon = new();
            Tue = new();
            Wed = new();
            Thu = new();
            Fri = new();
            Sat = new();
        }
    }

    public class DayInfo
    {
        public int Date { get; set; }
        public string Day { get; set; } = string.Empty;
        public string ToolTip { get; set; } = string.Empty;
        public EmrCalendarDateColor Foreground { get; set; }
        public EmrCalendarDateColor Background { get; set; }
        public EmrCalendarDateColor BorderBrush { get; set; }
        public bool IsToday { get; set; }

        public DayInfo (int date, string day, string toolTip, EmrCalendarDateColor foreground, EmrCalendarDateColor background, EmrCalendarDateColor borderBrush, bool isToday)
        {
            Date = date;
            Day = day ?? string.Empty;
            ToolTip = toolTip ?? string.Empty;
            Foreground = foreground;
            Background = background;
            BorderBrush = borderBrush;
            IsToday = isToday;
        }
        public DayInfo () { }
    }
}
