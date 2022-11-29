using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Enum
{
    public enum CheckSpecialType
    {
        /// <summary>
        /// 年齢制限
        /// </summary>
        AgeLimit = 1,

        /// <summary>
        /// 有効期限
        /// </summary>
        Expiration = 2,

        /// <summary>
        /// 算定回数
        /// </summary>
        CalculationCount = 3,

        /// <summary>
        /// コメント
        /// </summary>
        ItemComment = 4,

        /// <summary>
        /// 項目重複
        /// </summary>
        Duplicate = 5,
    }

    public enum CheckAgeType
    {
        MaxAge,
        MinAge
    }
}
