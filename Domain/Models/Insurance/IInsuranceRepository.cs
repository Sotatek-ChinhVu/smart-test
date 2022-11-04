﻿using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate);

        InsuranceModel GetPtHokenInf(int hpId, int hokenPid, long ptId, int sinDate);

        IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, bool allowDisplayDeleted, bool isAllHoken = true, bool isHoken = true, bool isJihi = true, bool isRosai = true, bool isJibai = true);

        bool CheckHokenPIdList(List<int> hokenPIds, List<int> hpIds, List<long> ptIds);

        bool CheckHokenPid(int hokenPId);

        List<HokenInfModel> GetCheckListHokenInf(int hpId, long ptId, List<int> hokenPids);

        int GetDefaultSelectPattern(int hpId, long ptId, int sinDate, int historyPid, int selectedHokenPid);

        List<InsuranceModel> GetInsuranceList(int hpId, long ptId, int sinDate);
    }
}
