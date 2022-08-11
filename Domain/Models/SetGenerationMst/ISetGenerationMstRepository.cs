namespace Domain.Models.SetGenerationMst
{
    public interface ISetGenerationMstRepository
    {
        IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate);
        int GetGenerationId(int hpId, int sinDate);
    }
}
