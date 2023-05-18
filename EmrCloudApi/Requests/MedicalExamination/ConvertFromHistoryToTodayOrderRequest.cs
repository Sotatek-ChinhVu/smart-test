﻿using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ConvertFromHistoryToTodayOrderRequest
    {
        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public int SanteiKbn { get; set; }

        public long PtId { get; set; }

        public List<OdrInfItemOfTodayOrder> HistoryOdrInfModels { get; set; } = new();
    }
}
