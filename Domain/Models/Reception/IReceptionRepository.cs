namespace Domain.Models.Reception
{
    public interface IReceptionRepository
    {
        ReceptionModel? Get(long raiinNo);
        List<ReceptionRowModel> GetList(int hpId, int sinDate);
    }
}
