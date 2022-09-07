﻿using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate);

        IEnumerable<InsuranceModel> GetListPokenPattern(int hpId, long ptId, bool allowDisplayDeleted);

        bool CheckHokenPIdList(List<int> hokenPIds);
    }
}
