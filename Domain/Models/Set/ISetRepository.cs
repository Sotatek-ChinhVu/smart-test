namespace Domain.Models.Set
{
    public interface ISetRepository
    {
        IEnumerable<SetMst> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch);
    }
}
