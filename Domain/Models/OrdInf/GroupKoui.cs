using Domain.CommonObject;
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfs
{
    public class GroupKoui : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private GroupKoui(OdrKouiKbn value)
        {
            _value = GetGroupKoui(value);
        }

        public static GroupKoui From(OdrKouiKbn value)
        {
            return new GroupKoui(value);
        }

        private int GetGroupKoui(
       OdrKouiKbn odrKouiKbn)
        {
            var result = odrKouiKbn.Value / 10 * 10;

            if (11 <= odrKouiKbn.Value && odrKouiKbn.Value <= 13)
            {
                // NuiTran recommend handle this case
                result = 11;
            }
            else if (odrKouiKbn.Value == 14)
            {
                // 在宅
                result = odrKouiKbn.Value;
            }
            else if (odrKouiKbn.Value >= 68 && odrKouiKbn.Value < 70)
            {
                // 画像
                result = odrKouiKbn.Value;
            }
            else if (odrKouiKbn.Value >= 95 && odrKouiKbn.Value < 99)
            {
                // コメント以外
                result = odrKouiKbn.Value;
            }
            else if (odrKouiKbn.Value == 100 || odrKouiKbn.Value == 101)
            {
                // コメント（処方箋）
                // コメント（処方箋備考）
                result = 20;
            }

            return result;
        }
    }
}
