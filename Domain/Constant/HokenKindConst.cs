using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    /// <summary>
    /// 保険の種類
    /// </summary>
    public class HokenSyu
    {
        /// <summary>
        /// 健保
        /// </summary>
        public const int Kenpo = 0;
        /// <summary>
        /// 労災
        /// </summary>
        public const int Rosai = 1;
        /// <summary>
        /// アフターケア
        /// </summary>
        public const int After = 2;
        /// <summary>
        /// 自賠
        /// </summary>
        public const int Jibai = 3;
        /// <summary>
        /// 自費
        /// </summary>
        public const int Jihi = 4;
    }
}
