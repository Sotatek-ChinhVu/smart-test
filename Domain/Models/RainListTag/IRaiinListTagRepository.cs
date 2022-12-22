using Domain.Common;

namespace Domain.Models.RainListTag
{
    public interface IRaiinListTagRepository : IRepositoryBase
    {
        IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar, List<int> sinDates, List<long> raiinNos);

        IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar);

        List<RaiinListTagModel> GetList(int hpId, long ptId, List<long> raiinNoList);

        RaiinListTagModel Get(int hpId, long ptId, long raiinNo, int sinDate);
    }
}
