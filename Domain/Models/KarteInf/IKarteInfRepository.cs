namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);

        List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos);

        int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText);

        long SaveListFileKarte(int hpId, long ptId, long raiinNo, long lastSeqNo, List<KarteImgInfModel> listModel, List<long> listFileDeletes);

        bool CheckExistListFile(int hpId, long ptId, long seqNo, long rainNo, List<long> listFileDeletes);

        long GetLastSeqNo(int hpId, long ptId, long rainNo);

        List<KarteImgInfModel> GetListKarteFile(int hpId, long ptId, long rainNo);
    }
}
