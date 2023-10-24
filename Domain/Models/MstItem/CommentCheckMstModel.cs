using Helper.Extension;

namespace Domain.Models.MstItem
{
    public class CommentCheckMstModel
    {

        public CommentCheckMstModel(
            string itemCd,
            string tenMstName,
            string kanaName1,
            string kanaName2,
            string kanaName3,
            string kouiName,
            int kohatuKbn,
            double ten,
            int tenId
        )
        {
            ItemCd = itemCd;
            KanaName3 = kanaName3;
            TenMstName = tenMstName;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KouiName = kouiName;
            KohatuKbn = kohatuKbn;
            Ten = ten;
            TenId = tenId;
        }

        public string TenMstName { get; private set; }

        public string KanaName1 { get; private set; }

        public string KanaName2 { get; private set; }

        public string KanaName3 { get; private set; }

        public string KouiName { get; private set; }

        public int KohatuKbn { get; private set; }

        public double Ten { get; private set; }

        public int TenId { get; private set; }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int IsDeleted { get; private set; }

        public string KohatuKbnDisplay
        {
            get
            {
                if (KohatuKbn == 1)
                {
                    return "後";
                }
                else if (KohatuKbn == 2)
                {
                    return "〇";
                }
                return string.Empty;
            }
        }

        public string TenDisplay
        {
            get
            {
                if (Ten == 0) return Ten.AsString();

                if (new[] { 1, 2, 4 }.Contains(TenId))
                {
                    return "￥" + Ten.AsString();
                }
                if (new[] { 5, 6 }.Contains(TenId))
                {
                    return Ten.AsString() + "%";
                }
                return Ten.AsString();
            }
        }
    }
}
