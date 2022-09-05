﻿using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int sinDate);
        IEnumerable<InsuranceModel> GetListPokenPattern(int hpId, long ptId, int deleteCondition);
        bool CheckHokenPIdList(List<int> hokenPIds);
    }
}
