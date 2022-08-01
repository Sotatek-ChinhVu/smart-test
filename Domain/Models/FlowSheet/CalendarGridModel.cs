using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class CalendarGridModel
    {
        public ObservableCollection<WeekOfMonthModel> WeekOfMonths { get; set; }
        public DateTime MonthYear { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int MonthTextMode { get; set; } = 1;
        public bool HighlightToday { get; set; } = true;
        public bool HighCurrentMonth { get; set; }
        public string MonthText { get; set; }
        public string MonthFontWeight { get; set; } = "Normal";
        public List<KeyValuePair<int, int>> RaiinDayDict { get; set; }
        public Dictionary<int, string> Holidays { get; set; }
        public List<HolidayModel> HolidayModels { get; set; }
        public int SinDate { get; set; }
        public List<KeyValuePair<int, string>> RaiinTags { get; set; }
        public List<KeyValuePair<int, RaiinStateDictObjectValue>> RaiinStateDict { get; set; }

        public CalendarGridModel(DateTime date)
        {
            WeekOfMonths = new();
            MonthYear = date;
            Month = MonthYear.Month;
            Year = MonthYear.Year;
            if (MonthTextMode == 1)
            {
                MonthText = string.Format("{0}年{1}月", Year, Month);
            }
            else
            {
                MonthText = string.Format("{0}月", Month);
            }
            RaiinDayDict = new();
            Holidays = new();
            HolidayModels = new();
            SinDate = 0;
            RaiinTags = new();
            RaiinStateDict = new();
        }
    }
    public class RaiinStateDictObjectValue
    {
        public int Value { get; set; }
        public long RaiinNo { get; set; }

        public RaiinStateDictObjectValue(int value, long raiinNo)
        {
            Value = value;
            RaiinNo = raiinNo;
        }
    }
}
