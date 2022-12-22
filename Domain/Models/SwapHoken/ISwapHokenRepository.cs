using Domain.Common;

namespace Domain.Models.SwapHoken
{
    public interface ISwapHokenRepository : IRepositoryBase
    {
        long CountOdrInf(int hpId, long ptId, int hokenPid, int startDate, int endDate);

        List<int> GetListSeikyuYms(int hpId, long ptId, int hokenPid, int startDate, int endDate);

        List<int> GetSeikyuYmsInPendingSeikyu(int hpId, long ptId, List<int> sinYms, int hokenId);

        bool SwapHokenParttern(int hpId, long PtId, int OldHokenPid, int NewHokenPid, int StartDate, int EndDate, int userId);

        bool ExistRaiinInfUsedOldHokenId(int hpId, long ptId, List<int> sinYms, int oldHokenPId);

        bool UpdateReceSeikyu(int hpId, long ptId, List<int> seiKyuYms, int oldHokenId, int newHokenId, int userId);
    }
}