namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);

        List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long>? raiinNos);

        bool SaveListImageKarteImgTemp(List<KarteImgInfModel> listModel);
    }
}
