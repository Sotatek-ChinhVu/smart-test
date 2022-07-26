namespace Domain.Models.SetKbn
{
    public interface ISetKbnMstRepository
    {
        IEnumerable<SetKbnMst> GetList(int hpId, int setKbnFrom, int setKbnTo);
    }
}
