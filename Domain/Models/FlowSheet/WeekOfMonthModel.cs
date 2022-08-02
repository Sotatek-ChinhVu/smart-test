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
        public DayInfo Sun { get; private set; }
        public DayInfo Mon { get; private set; }
        public DayInfo Tue { get; private set; }
        public DayInfo Wed { get; private set; }
        public DayInfo Thu { get; private set; }
        public DayInfo Fri { get; private set; }
        public DayInfo Sat { get; private set; }
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
        public int Date { get; private set; }
        public string Day { get; private set; } = string.Empty;
        public string ToolTip { get; private set; } = string.Empty;
        public EmrCalendarDateColor Foreground { get; private set; }
        public EmrCalendarDateColor Background { get; private set; }
        public EmrCalendarDateColor BorderBrush { get; private set; }
        public bool IsToday { get; private set; }

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
