namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);

        List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos);

        int GetSinDate(long ptId, int hpId, int searchType, long raiinNo, string searchText);

        bool SaveListImageKarteImgTemp(List<KarteImgInfModel> listModel);
    }
}
