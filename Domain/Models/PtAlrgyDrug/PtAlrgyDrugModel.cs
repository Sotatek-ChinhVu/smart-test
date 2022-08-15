﻿namespace Domain.Models.PtAlrgyDrug
{
    public class PtAlrgyDrugModel
    {
        public PtAlrgyDrugModel(int hpId, long ptId, int seqNo, int sortNo, string itemCd, string drugName, int startDate, int endDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ItemCd = itemCd;
            DrugName = drugName;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SeqNo { get; private set; }
        public int SortNo { get; private set; }
        public string ItemCd { get; private set; }
        public string DrugName { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string Cmt { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
