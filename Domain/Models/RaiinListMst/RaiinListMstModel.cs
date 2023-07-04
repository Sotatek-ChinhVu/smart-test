﻿namespace Domain.Models.RaiinListMst
{
    public class RaiinListMstModel
    {
        public RaiinListMstModel(int grpId, string grpName, int sortNo, int isDeleted, List<RaiinListDetailModel> raiinListDetailsList)
        {
            GrpId = grpId;
            GrpName = grpName;
            SortNo = sortNo;
            RaiinListDetailsList = raiinListDetailsList;
            IsDeleted = isDeleted;
        }

        public int GrpId { get; private set; }

        public string GrpName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RaiinListDetailModel> RaiinListDetailsList { get; private set; }
    }
}
