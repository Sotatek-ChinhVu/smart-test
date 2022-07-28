using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public static class TenkiKbnConst
    {
        /// <summary>
        /// 継続
        /// </summary>
        public const int Continued = 0;
        /// <summary>
        /// 治ゆ
        /// </summary>
        public const int Cured = 1;
        /// <summary>
        /// 中止
        /// </summary>
        public const int Canceled = 2;
        /// <summary>
        /// 死亡
        /// </summary>
        public const int Dead = 3;
        /// <summary>
        /// 当月
        /// </summary>
        public const int InMonth = 4;

        /// <summary>
        /// その他
        /// </summary>
        public const int Other = 9;

        public static Dictionary<int, string> TenkiKbnDict { get; } = new Dictionary<int, string>()
        {
            {Continued,"0 継続" },
            {Cured,"1 治ゆ" },
            {Canceled,"2 中止" },
            {Dead,"3 死亡" },
            {InMonth,"4 当月" },
            {Other,"9 その他" },
        };

        public static Dictionary<int, string> DisplayedTenkiKbnDict { get; } = new Dictionary<int, string>()
        {
            {Continued,"継続" },
            {Cured,"治ゆ" },
            {Canceled,"中止" },
            {Dead,"死亡" },
            {InMonth,"当月" },
            {Other,"その他" },
        };
    }
}
