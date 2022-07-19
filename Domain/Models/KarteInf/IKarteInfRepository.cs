namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);
    }
}
