﻿using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Requests.InsuranceMst
{
    public class HokenMasterDto
    {
        public HokenMasterDto(int futanKbn, int futanRate, int startDate, int endDate, int hokenNo, int hokenEdaNo, string hokenSName, string houbetu, int hokenSbtKbn, int checkDigit, int ageStart, int ageEnd, int isFutansyaNoCheck, int isJyukyusyaNoCheck, int jyuKyuCheckDigit, int isTokusyuNoCheck, string hokenName, string hokenNameCd, int hokenKohiKbn, int isOtherPrefValid, int receKisai, int isLimitList, int isLimitListSum, int enTen, int kaiLimitFutan, int dayLimitFutan, int monthLimitFutan, int monthLimitCount, int limitKbn, int countKbn, int futanYusen, int calcSpKbn, int monthSpLimit, int kogakuTekiyo, int kogakuTotalKbn, int kogakuHairyoKbn, int receSeikyuKbn, int receKisaiKokho, int receKisai2, int receTenKisai, int receFutanRound, int receZeroKisai, int receSpKbn, int prefNo, int sortNo, int seikyuYm, int receFutanHide, int receFutanKbn, int kogakuTotalAll, int kogakuTotalExcFutan, int kaiFutangaku, int dayLimitCount, bool isAdded, List<ExceptHokensyaModel> excepHokenSyas)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            StartDate = startDate;
            EndDate = endDate;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenSName = hokenSName;
            Houbetu = houbetu;
            HokenSbtKbn = hokenSbtKbn;
            CheckDigit = checkDigit;
            AgeStart = ageStart;
            AgeEnd = ageEnd;
            IsFutansyaNoCheck = isFutansyaNoCheck;
            IsJyukyusyaNoCheck = isJyukyusyaNoCheck;
            JyuKyuCheckDigit = jyuKyuCheckDigit;
            IsTokusyuNoCheck = isTokusyuNoCheck;
            HokenName = hokenName;
            HokenNameCd = hokenNameCd;
            HokenKohiKbn = hokenKohiKbn;
            IsOtherPrefValid = isOtherPrefValid;
            ReceKisai = receKisai;
            IsLimitList = isLimitList;
            IsLimitListSum = isLimitListSum;
            EnTen = enTen;
            KaiLimitFutan = kaiLimitFutan;
            DayLimitFutan = dayLimitFutan;
            MonthLimitFutan = monthLimitFutan;
            MonthLimitCount = monthLimitCount;
            LimitKbn = limitKbn;
            CountKbn = countKbn;
            FutanYusen = futanYusen;
            CalcSpKbn = calcSpKbn;
            MonthSpLimit = monthSpLimit;
            KogakuTekiyo = kogakuTekiyo;
            KogakuTotalKbn = kogakuTotalKbn;
            KogakuHairyoKbn = kogakuHairyoKbn;
            ReceSeikyuKbn = receSeikyuKbn;
            ReceKisaiKokho = receKisaiKokho;
            ReceKisai2 = receKisai2;
            ReceTenKisai = receTenKisai;
            ReceFutanRound = receFutanRound;
            ReceZeroKisai = receZeroKisai;
            ReceSpKbn = receSpKbn;
            PrefNo = prefNo;
            SortNo = sortNo;
            SeikyuYm = seikyuYm;
            ReceFutanHide = receFutanHide;
            ReceFutanKbn = receFutanKbn;
            KogakuTotalAll = kogakuTotalAll;
            KogakuTotalExcFutan = kogakuTotalExcFutan;
            KaiFutangaku = kaiFutangaku;
            DayLimitCount = dayLimitCount;
            IsAdded = isAdded;
            ExcepHokenSyas = excepHokenSyas;
        }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string HokenSName { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public int CheckDigit { get; private set; }

        public int AgeStart { get; private set; }

        public int AgeEnd { get; private set; }

        public int IsFutansyaNoCheck { get; private set; }

        public int IsJyukyusyaNoCheck { get; private set; }

        public int JyuKyuCheckDigit { get; private set; }

        public int IsTokusyuNoCheck { get; private set; }

        public string HokenName { get; private set; }

        public string HokenNameCd { get; private set; }

        public int HokenKohiKbn { get; private set; }

        public int IsOtherPrefValid { get; private set; }

        public int ReceKisai { get; private set; }

        public int IsLimitList { get; private set; }

        public int IsLimitListSum { get; private set; }

        public int EnTen { get; private set; }

        public int KaiLimitFutan { get; private set; }

        public int DayLimitFutan { get; private set; }

        public int MonthLimitFutan { get; private set; }

        public int MonthLimitCount { get; private set; }

        public int LimitKbn { get; private set; }

        public int CountKbn { get; private set; }

        public int FutanYusen { get; private set; }

        public int CalcSpKbn { get; private set; }

        public int MonthSpLimit { get; private set; }

        public int KogakuTekiyo { get; private set; }

        public int KogakuTotalKbn { get; private set; }

        public int KogakuHairyoKbn { get; private set; }

        public int ReceSeikyuKbn { get; private set; }

        public int ReceKisaiKokho { get; private set; }

        public int ReceKisai2 { get; private set; }

        public int ReceTenKisai { get; private set; }

        public int ReceFutanRound { get; private set; }

        public int ReceZeroKisai { get; private set; }

        public int ReceSpKbn { get; private set; }

        public int PrefNo { get; private set; }

        public int SortNo { get; private set; }

        public int SeikyuYm { get; private set; }

        public int ReceFutanHide { get; private set; }

        public int ReceFutanKbn { get; private set; }

        public int KogakuTotalAll { get; private set; }

        public int KogakuTotalExcFutan { get; private set; }

        public int KaiFutangaku { get; private set; }

        public int DayLimitCount { get; private set; }

        public bool IsAdded { get; private set; }

        public List<ExceptHokensyaModel> ExcepHokenSyas { get; private set; } = new List<ExceptHokensyaModel>();
    }
}
