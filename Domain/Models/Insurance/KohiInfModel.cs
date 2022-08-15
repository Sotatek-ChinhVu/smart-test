﻿namespace Domain.Models.Insurance
{
    public class KohiInfModel
    {
        public KohiInfModel(string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, HokenMstModel? hokenMstModel)
        {
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            Rate = rate;
            GendoGaku = gendoGaku;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            TokusyuNo = tokusyuNo;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            HokenMstModel = hokenMstModel;
        }

        public string FutansyaNo { get; private set; }
        public string JyukyusyaNo { get; private set; }
        public int HokenId { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public int ConfirmDate { get; private set; }
        public int Rate { get; private set; }
        public int GendoGaku { get; private set; }
        public int SikakuDate { get; private set; }
        public int KofuDate { get; private set; }
        public string TokusyuNo { get; private set; }
        public int HokenSbtKbn { get; private set; }
        public string Houbetu { get; private set; }
        public HokenMstModel? HokenMstModel { get; private set; }

    }
}
