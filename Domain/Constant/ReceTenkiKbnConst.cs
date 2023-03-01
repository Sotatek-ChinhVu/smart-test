using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public class ReceTenkiKbnConst
    {
        /// <summary>
        /// 継続
        /// </summary>
        public const int Continued = 1;
        /// <summary>
        /// 治ゆ
        /// </summary>
        public const int Cured = 2;
        /// <summary>
        /// 中止
        /// </summary>
        public const int Canceled = 4;
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
    }
}
