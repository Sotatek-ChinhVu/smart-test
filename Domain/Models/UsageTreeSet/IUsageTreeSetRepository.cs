namespace Domain.Models.UsageTreeSet
{
    public interface IUsageTreeSetRepository
    {
        int GetGenerationId(int sinDate);

        List<ListSetMstModel> GetTanSetInfs(int hpId, int setUsageKbn, int generationId,int sinDate);

        /// <summary></summary>
        /// <param name="hpId"></param>
        /// <param name="usageContains">usageDrug or usageInject or listMedicalManagement check constain property SetKbn </param>
        /// <param name="GenerationId"> GenerationId </param>
        /// <returns></returns>
        List<ListSetMstModel> GetTanSetInfs(int hpId, IEnumerable<int> usageContains, int generationId,int sinDate);
        List<ListSetMstModel> GetAllTanSetInfs(int hpId, int generationId, int sinDate);
    }
}