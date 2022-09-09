namespace Domain.Models.RainListTag
{
    public interface IRaiinListTagRepository
    {
        IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar);
    }
}
