namespace Helper.Constants
{
    public static class SyosaiConst
    {
        /// <summary>
        /// 初再診なし
        /// </summary>
        public const int None = 0;
        /// <summary>
        /// 初診
        /// </summary>
        public const int Syosin = 1;
        /// <summary>
        /// 指定なし
        /// </summary>
        public const int Unspecified = 2;
        /// <summary>
        /// 再診
        /// </summary>
        public const int Saisin = 3;
        /// <summary>
        /// 電話再診
        /// </summary>
        public const int SaisinDenwa = 4;
        /// <summary>
        /// 自費
        /// </summary>
        public const int Jihi = 5;
        /// <summary>
        /// 初診２科目
        /// </summary>
        public const int Syosin2 = 6;
        /// <summary>
        /// 再診２科目
        /// </summary>
        public const int Saisin2 = 7;
        /// <summary>
        /// 電話再診２科目
        /// </summary>
        public const int SaisinDenwa2 = 8;
        /// <summary>
        /// 初診コロナ
        /// </summary>
        public const int SyosinCorona = 91;
        /// <summary>
        /// 初診情報通信機器
        /// </summary>
        public const int SyosinJouhou = 81;
        /// <summary>
        /// 再診情報通信機器
        /// </summary>
        public const int SaisinJouhou = 83;
        /// <summary>
        /// 初診２科目情報通信機器
        /// </summary>
        public const int Syosin2Jouhou = 86;
        /// <summary>
        /// 再診２科目情報通信機器
        /// </summary>
        public const int Saisin2Jouhou = 87;
        /// <summary>
        /// Add Const defind next order for display next order tooltip in flowsheet calendar
        /// 次回オーダー
        /// </summary>
        public const int NextOrder = -1;

        public static Dictionary<int, string> ShinDict { get; } = new Dictionary<int, string>()
        {
            {Syosin, "初診" },
            {Syosin2, "同日初診" },
            {Saisin, "再診" },
            {Saisin2, "再診（２科目）" },
            {SaisinDenwa, "電話再診" },
            {SaisinDenwa2, "電話再診（２科目）" },
            {None, "なし" },
            {Jihi, "なし（×自動算定）" }
        };

        public static Dictionary<int, string> FlowSheetShinDict { get; } = new Dictionary<int, string>()
        {
            {None, "-" },
            {Syosin, "初" },
            {Unspecified, "-" },
            {Saisin, "再" },
            {SaisinDenwa, "電再" },
            {Jihi, "自費" },
            {Syosin2, "同初" },
            {Saisin2, "再２" },
            {SaisinDenwa2, "電２" },
        };

        public static Dictionary<int, string> ReceptionShinDict { get; } = new Dictionary<int, string>()
        {
            {Unspecified, "（指定なし）" },
            {Syosin, "初診" },
            {Syosin2, "同日初診" },
            {Saisin, "再診" },
            {Saisin2, "再診（２科目）" },
            {SaisinDenwa, "電話再診" },
            {SaisinDenwa2, "電話再診（２科目）" },
            {None, "なし" },
            {Jihi, "なし（×自動算定）" }
        };

        public static Dictionary<int, string> FlowSheetCalendarDict { get; } = new Dictionary<int, string>()
        {
            {Unspecified, "（指定なし）" },
            {Syosin, "初診" },
            {Syosin2, "同日初診" },
            {Saisin, "再診" },
            {Saisin2, "再診（２科目）" },
            {SaisinDenwa, "電話再診" },
            {SaisinDenwa2, "電話再診（２科目）" },
            {None, "なし" },
            {Jihi, "なし（×自動算定）" },
            {NextOrder, "予約オーダー" }
        };
    }
}
