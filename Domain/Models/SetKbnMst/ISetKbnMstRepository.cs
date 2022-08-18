namespace Domain.Models.SetKbnMst
{
    public interface ISetKbnMstRepository
    {
        IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo);
    }
}
