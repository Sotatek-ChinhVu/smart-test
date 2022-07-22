namespace Domain.Models.KarteKbn
{
    public interface IKarteKbnRepository
    {
        List<KarteKbnMst> GetList(int hpId, bool isDeleted);
    }
}
