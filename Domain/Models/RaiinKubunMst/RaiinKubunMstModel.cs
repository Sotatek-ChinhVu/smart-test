﻿
namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKubunMstModel
    {
        public int GroupId { get; private set; }

        public int SortNo { get; private set; }

        public string GroupName { get; private set; }

        public bool IsDeleted { get; private set; }

        public List<RaiinKubunDetailModel> RaiinKubunDetailModels { get; private set; }

        public RaiinKubunMstModel(int groupId, int sortNo, string groupName, bool isDeleted, List<RaiinKubunDetailModel> raiinKubunDetailModels)
        {
            GroupId = groupId;
            SortNo = sortNo;
            GroupName = groupName;
            IsDeleted = isDeleted;
            RaiinKubunDetailModels = raiinKubunDetailModels;
        }
    }
}
