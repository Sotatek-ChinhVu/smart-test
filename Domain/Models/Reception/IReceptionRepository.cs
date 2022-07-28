namespace Domain.Models.Reception
{
    public interface IReceptionRepository
    {
        ReceptionModel? Get(long raiinNo);
        List<ReceptionRowModel> GetList(int hpId, int sinDate);
        List<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory);

    }
}
