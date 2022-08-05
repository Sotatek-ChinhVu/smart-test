namespace Helper.Common;

public class UserConfCommon
{
    public enum DateTimeFormart
    {
        JapaneseCalendar = 0,
        WesternCalendar = 1,
        JapAndWestCalendar = 2,
    }

    public static class GroupCodes
    {
        /// <summary>
        /// Group code that contains font name and font size settings
        /// </summary>
        public const int Font = 2001;
        public const int AutoRefresh = 2002;
        public const int MouseWheel = 2003;
        public const int KanFocus = 2004;
        public const int SelectTodoSetting = 2005;
    }
}
