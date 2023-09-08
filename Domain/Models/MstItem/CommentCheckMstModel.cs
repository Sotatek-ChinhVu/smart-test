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
            TenMstName = tenMstName;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KouiName = kouiName;
            KohatuKbn = kohatuKbn;
            Ten = ten;
            TenId = tenId;
        }

        private string _tenMstName;
        public string TenMstName
        {
            get => _tenMstName;
            set
            {
                if (_tenMstName == value) return;
                _tenMstName = value;
            }
        }

        private string _kanaName1;
        public string KanaName1
        {
            get => _kanaName1;
            set
            {
                if (_kanaName1 == value) return;
                _kanaName1 = value;
            }
        }

        private string _kanaName2;
        public string KanaName2
        {
            get => _kanaName2;
            set
            {
                if (_kanaName2 == value) return;
                _kanaName2 = value;
            }
        }

        private string _kanaName3;
        public string KanaName3
        {
            get => _kanaName3;
            set
            {
                if (_kanaName3 == value) return;
                _kanaName3 = value;
            }
        }

        private string _kouiName;
        public string KouiName
        {
            get => _kouiName;
            set
            {
                if (_kouiName == value) return;
                _kouiName = value;
            }
        }

        private int _kohatuKbn;
        public int KohatuKbn
        {
            get => _kohatuKbn;
            set
            {
                if (_kohatuKbn == value) return;
                _kohatuKbn = value;
            }
        }

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

        private double _ten;
        public double Ten
        {
            get => _ten;
            set
            {
                if (_ten == value) return;
                _ten = value;
            }
        }

        private int _tenId;
        public int TenId
        {
            get => _tenId;
            set
            {
                if (_tenId == value) return;
                _tenId = value;
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

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
