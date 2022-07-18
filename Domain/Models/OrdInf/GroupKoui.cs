using Domain.Core;

namespace Domain.Models.OrdInfs
{
    public class GroupKoui : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private GroupKoui(int value)
        {
            _value = GetGroupKoui(value);
        }

        public static GroupKoui From(int value)
        {
            return new GroupKoui(value);
        }

        private int GetGroupKoui(
       int odrKouiKbn)
        {
            var result = odrKouiKbn / 10 * 10;

            if (11 <= odrKouiKbn && odrKouiKbn <= 13)
            {
                // NuiTran recommend handle this case
                result = 11;
            }
            else if (odrKouiKbn == 14)
            {
                // 在宅
                result = odrKouiKbn;
            }
            else if (odrKouiKbn >= 68 && odrKouiKbn < 70)
            {
                // 画像
                result = odrKouiKbn;
            }
            else if (odrKouiKbn >= 95 && odrKouiKbn < 99)
            {
                // コメント以外
                result = odrKouiKbn;
            }
            else if (odrKouiKbn == 100 || odrKouiKbn == 101)
            {
                // コメント（処方箋）
                // コメント（処方箋備考）
                result = 20;
            }

            return result;
        }
    }
}
