namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);

        List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos);

        int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText);

        bool SaveListFileKarte(int hpId, long ptId, long raiinNo, List<string> listFileName, bool saveTempFile);

        bool CheckExistListFile(int hpId, long ptId, long seqNo, long rainNo, List<long> listFileDeletes);

        long GetLastSeqNo(int hpId, long ptId, long rainNo);

        List<string> GetListKarteFile(int hpId, long ptId, long raiinNo, bool searchTempFile);

        bool ClearTempData(int hpId, long ptId, List<string> listFileNames);
    }
}
