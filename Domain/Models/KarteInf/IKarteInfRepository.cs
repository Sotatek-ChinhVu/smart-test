namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInf> GetList(long ptId, long rainNo, long sinDate);
    }
}
