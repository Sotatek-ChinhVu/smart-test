﻿using Domain.Common;
using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository : IRepositoryBase
    {
        InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate);

        InsuranceModel GetPtHokenInf(int hpId, int hokenPid, long ptId, int sinDate);

        IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, int sinDate, bool allowDisplayDeleted, bool isAllHoken = true, bool isHoken = true, bool isJihi = true, bool isRosai = true, bool isJibai = true);

        bool CheckExistHokenPIdList(List<int> hokenPIds, List<int> hpIds, List<long> ptIds);

        bool CheckExistHokenId(int hokenId);

        bool CheckExistHokenPid(int hokenPid);

        List<HokenInfModel> GetCheckListHokenInf(int hpId, long ptId, List<int> hokenPids);

        List<int> GetListHistoryPid(int hpId, long ptId, int sinDate, List<int> historyPids, int selectedHokenPid);

        List<InsuranceModel> GetInsuranceList(int hpId, long ptId, int sinDate, bool isDeleted = false);

        bool DeleteInsuranceScan(int hpId, long seqNo, int userId);

        bool CheckHokenPatternUsed(int hpId, long ptId, int hokenPid);

        List<KohiPriorityModel> GetKohiPriorityList();

        List<InsuranceScanModel> GetListInsuranceScanByPtId(int hpId, long ptId);

        int GetHokenKbnByHokenId(int hpId, int hokenId, long ptId);
    }
}
