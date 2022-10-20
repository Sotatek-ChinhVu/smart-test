namespace Domain.Models.RainListTag
{
    public interface IRaiinListTagRepository
    {
        IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar, List<int> sinDates, List<long> raiinNos);

        IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar);

        RaiinListTagModel Get(int hpId, long ptId, long raiinNo, int sinDate);
    }
}
