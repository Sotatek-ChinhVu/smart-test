﻿using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.CheckedSpecialItem;
using static Helper.Constants.OrderInfConst;

namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class CheckedOrderItem
    {
        public CheckingType CheckingType { get; set; }

        public string CheckingTypeDisplay { get; set; } = string.Empty;

        public bool Santei { get; set; }

        public string CheckingContent { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public int SinKouiKbn { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int InOutKbn { get; set; }
    }
}