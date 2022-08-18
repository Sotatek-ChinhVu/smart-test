using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Constant
{
    public static class JikanConst
    {
        public const int UnSet = -1;
        /// <summary>
        /// 時間内
        /// </summary>
        public const int JikanNai = 0;
        /// <summary>
        /// 時間外
        /// </summary>
        public const int JikanGai = 1;
        /// <summary>
        /// 休日
        /// </summary>
        public const int Kyujitu = 2;
        /// <summary>
        /// 深夜
        /// </summary>
        public const int Sinya = 3;
        /// <summary>
        /// 夜間早朝
        /// </summary>
        public const int Yasou = 4;

        /// <summary>
        /// 夜間小特
        /// </summary>
        public const int YakanKotoku = 5;

        /// <summary>
        /// 休日小特
        /// </summary>
        public const int KyujituKotoku = 6;

        /// <summary>
        /// 深夜小特
        /// </summary>
        public const int SinyaKotoku = 7;

        public static Dictionary<int, string> JikanDict { get; } = new Dictionary<int, string>()
        {
            {JikanNai, "時間内" },
            {JikanGai, "時間外" },
            {Kyujitu, "休日" },
            {Sinya, "深夜" },
            {Yasou, "夜・早" }
        };

        public static Dictionary<int, string> JikanKotokuDict { get; } = new Dictionary<int, string>()
        {
            {JikanNai, "時間内" },
            {JikanGai, "時間外" },
            {Kyujitu, "休日" },
            {Sinya, "深夜" },
            {Yasou, "夜・早" },
            {YakanKotoku, "夜間小特" },
            {KyujituKotoku, "休日小特" },
            {SinyaKotoku, "深夜小特" }
        };
    }
}
