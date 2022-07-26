namespace Domain.Models.SetGeneration
{
    public interface ISetGenerationRepository
    {
        IEnumerable<SetGenerationMst> GetList(int hpId, int sinDate);
        int GetGenerationId(int hpId, int sinDate);
    }
}
