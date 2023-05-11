namespace Domain.Models.TimeZone
{
    public class TimeZoneConfGroupModel
    {
        public TimeZoneConfGroupModel(int youbiKbn, IEnumerable<TimeZoneConfModel> details)
        {
            YoubiKbn = youbiKbn;
            Details = details.OrderBy(u => u.SortNo).ToList();
        }

        /// <summary>
        /// 曜日区分
        ///     1..7:日曜～土曜
        /// </summary>
        public int YoubiKbn { get; private set; }

        public List<TimeZoneConfModel> Details { get; private set; }

        public bool IsSaturDay
        {
            get => YoubiKbn == 7;
        }

        public bool IsSunDay
        {
            get => YoubiKbn == 1;
        }

        public string YoubiKbnDisplay
        {
            get
            {
                switch (YoubiKbn)
                {
                    case 1:
                        return "日曜日";
                    case 2:
                        return "月曜日";
                    case 3:
                        return "火曜日";
                    case 4:
                        return "水曜日";
                    case 5:
                        return "木曜日";
                    case 6:
                        return "金曜日";
                    case 7:
                        return "土曜日";
                }
                return string.Empty;
            }
        }
    }
}
