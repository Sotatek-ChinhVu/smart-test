namespace Domain.Models.HokenMst
{
    public class HokenMasterModel
    {
        public HokenMasterModel(int hpId, int hokenNo, int hokenEdaNo, int startDate, int endDate, int prefNo, string houbetu, string hokenName, string hokenNameCd, string hokenSname, int hokenSbtKbn, int hokenKohiKbn, int isLimitList, int isLimitListSum, int checkDigit, int jyukyuCheckDigit, int isFutansyaNoCheck, int isJyukyusyaNoCheck, int isTokusyuNoCheck, int ageStart, int ageEnd, int isOtherPrefValid, int enTen, int futanKbn, int futanRate, int kaiLimitFutan, int dayLimitFutan, int dayLimitCount, int monthLimitFutan, int monthLimitCount, int limitKbn, int countKbn, int calcSpKbn, int monthSpLimit, int kogakuTekiyo, int kogakuTotalKbn, int futanYusen, int receSeikyuKbn, int receKisai, int receKisai2, int receTenKisai, int receFutanHide, int receFutanRound, int receZeroKisai, int receSpKbn, string roudou)
        {
            HpId = hpId;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
            EndDate = endDate;
            PrefNo = prefNo;
            Houbetu = houbetu;
            HokenName = hokenName;
            HokenNameCd = hokenNameCd;
            HokenSname = hokenSname;
            HokenSbtKbn = hokenSbtKbn;
            HokenKohiKbn = hokenKohiKbn;
            IsLimitList = isLimitList;
            IsLimitListSum = isLimitListSum;
            CheckDigit = checkDigit;
            JyukyuCheckDigit = jyukyuCheckDigit;
            IsFutansyaNoCheck = isFutansyaNoCheck;
            IsJyukyusyaNoCheck = isJyukyusyaNoCheck;
            IsTokusyuNoCheck = isTokusyuNoCheck;
            AgeStart = ageStart;
            AgeEnd = ageEnd;
            IsOtherPrefValid = isOtherPrefValid;
            EnTen = enTen;
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            KaiLimitFutan = kaiLimitFutan;
            DayLimitFutan = dayLimitFutan;
            DayLimitCount = dayLimitCount;
            MonthLimitFutan = monthLimitFutan;
            MonthLimitCount = monthLimitCount;
            LimitKbn = limitKbn;
            CountKbn = countKbn;
            CalcSpKbn = calcSpKbn;
            MonthSpLimit = monthSpLimit;
            KogakuTekiyo = kogakuTekiyo;
            KogakuTotalKbn = kogakuTotalKbn;
            FutanYusen = futanYusen;
            ReceSeikyuKbn = receSeikyuKbn;
            ReceKisai = receKisai;
            ReceKisai2 = receKisai2;
            ReceTenKisai = receTenKisai;
            ReceFutanHide = receFutanHide;
            ReceFutanRound = receFutanRound;
            ReceZeroKisai = receZeroKisai;
            ReceSpKbn = receSpKbn;
            Roudou = roudou;
        }

        public HokenMasterModel()
        {
            HpId = 0;
            HokenNo = 0;
            HokenEdaNo = 0;
            StartDate = 0;
            EndDate = 0;
            PrefNo = 0;
            Houbetu = string.Empty;
            HokenName = string.Empty;
            HokenNameCd = string.Empty;
            HokenSname = string.Empty;
            HokenSbtKbn = 0;
            HokenKohiKbn = 0;
            IsLimitList = 0;
            IsLimitListSum = 0;
            CheckDigit = 0;
            JyukyuCheckDigit = 0;
            IsFutansyaNoCheck = 0;
            IsJyukyusyaNoCheck = 0;
            IsTokusyuNoCheck = 0;
            AgeStart = 0;
            AgeEnd = 0;
            IsOtherPrefValid = 0;
            EnTen = 0;
            FutanKbn = 0;
            FutanRate = 0;
            KaiLimitFutan = 0;
            DayLimitFutan = 0;
            DayLimitCount = 0;
            MonthLimitFutan = 0;
            MonthLimitCount = 0;
            LimitKbn = 0;
            CountKbn = 0;
            CalcSpKbn = 0;
            MonthSpLimit = 0;
            KogakuTekiyo = 0;
            KogakuTotalKbn = 0;
            FutanYusen = 0;
            ReceSeikyuKbn = 0;
            ReceKisai = 0;
            ReceKisai2 = 0;
            ReceTenKisai = 0;
            ReceFutanHide = 0;
            ReceFutanRound = 0;
            ReceZeroKisai = 0;
            ReceSpKbn = 0;
            Roudou = string.Empty;
        }

        public int HpId
        {
            get; private set;
        }

        public int HokenNo
        {
            get; private set;
        }

        public string DisplayHokenNo
        {
            get
            {
                return HokenNo.ToString().PadLeft(3, '0');
            }
        }

        public int HokenEdaNo
        {
            get; private set;
        }

        public int StartDate
        {
            get; private set;
        }

        public int EndDate
        {
            get; private set;
        }

        public int PrefNo
        {
            get; private set;
        }

        public string Houbetu
        {
            get; private set;
        }

        public string HoubetuDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(Houbetu))
                    return "0";
                return Houbetu.PadLeft(2, '0');
            }
        }

        public string HokenName
        {
            get; private set;
        }

        public string HokenNameCd
        {
            get; private set;
        }

        public string HokenSname
        {
            get; private set;
        }

        public int HokenSbtKbn
        {
            get; private set;
        }

        public int HokenKohiKbn
        {
            get; private set;
        }

        public int IsLimitList
        {
            get; private set;
        }

        public int IsLimitListSum
        {
            get; private set;
        }

        public int CheckDigit
        {
            get; private set;
        }

        public int JyukyuCheckDigit
        {
            get; private set;
        }

        public int IsFutansyaNoCheck
        {
            get; private set;
        }

        public int IsJyukyusyaNoCheck
        {
            get; private set;
        }

        public int IsTokusyuNoCheck
        {
            get; private set;
        }

        public int AgeStart
        {
            get; private set;
        }

        public int AgeEnd
        {
            get; private set;
        }

        public int IsOtherPrefValid
        {
            get; private set;
        }

        public int EnTen
        {
            get; private set;
        }

        public int FutanKbn
        {
            get; private set;
        }

        public int FutanRate
        {
            get; private set;
        }

        public int KaiLimitFutan
        {
            get; private set;
        }

        public int DayLimitFutan
        {
            get; private set;
        }

        public int DayLimitCount
        {
            get; private set;
        }

        public int MonthLimitFutan
        {
            get; private set;
        }

        public int MonthLimitCount
        {
            get; private set;
        }

        public int LimitKbn
        {
            get; private set;
        }

        public int CountKbn
        {
            get; private set;
        }

        public int CalcSpKbn
        {
            get; private set;
        }

        public int MonthSpLimit
        {
            get; private set;
        }

        public int KogakuTekiyo
        {
            get; private set;
        }

        public int KogakuTotalKbn
        {
            get; private set;
        }

        public int FutanYusen
        {
            get; private set;
        }

        public int ReceSeikyuKbn
        {
            get; private set;
        }

        public bool IsEnableReceSeikyuKbn
        {
            get => ReceSeikyuKbn >= 4;
        }

        public int ReceKisai
        {
            get; private set;
        }

        public int ReceKisai2
        {
            get; private set;
        }

        public int ReceTenKisai
        {
            get; private set;
        }

        public int ReceFutanHide
        {
            get; private set;
        }

        public int ReceFutanRound
        {
            get; private set;
        }

        public int ReceZeroKisai
        {
            get; private set;
        }

        public int ReceSpKbn
        {
            get; private set;
        }

        public string Roudou
        {
            get; set;
        }

        /// <summary>
        /// 点数単価
        /// 労災と自賠だけ編集できる
        /// </summary>
        public bool IsReadOnlyTensu
        {
            get => HokenSbtKbn != 3 && HokenSbtKbn != 4;
        }
    }
}