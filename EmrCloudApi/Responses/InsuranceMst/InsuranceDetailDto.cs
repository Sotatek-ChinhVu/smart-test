﻿namespace EmrCloudApi.Responses.InsuranceMst
{
    public class InsuranceDetailDto
    {
        public InsuranceDetailDto(int indicator, int sort, string acronymName, int prefNo, int hokenEdaNo, int startDate)
        {
            Indicator = indicator;
            Sort = sort;
            AcronymName = acronymName;
            PrefNo = prefNo;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
        }

        public int Indicator { get; private set; }

        public int Sort { get; private set; }

        public string AcronymName { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int StartDate { get; private set; }
    }
}
