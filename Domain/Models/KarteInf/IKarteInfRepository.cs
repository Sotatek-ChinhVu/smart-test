using Domain.Common;
using Domain.Models.KarteInf;

namespace Domain.Models.KarteInfs
{
    public interface IKarteInfRepository : IRepositoryBase
    {
        List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted);

        List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos);

        int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText);

        bool SaveListFileKarte(int hpId, long ptId, long raiinNo, string host, List<FileInfModel> listFiles, bool saveTempFile);

        long GetLastSeqNo(int hpId, long ptId, long rainNo);

        List<FileInfModel> GetListKarteFile(int hpId, long ptId, long raiinNo, bool searchTempFile);

        List<FileInfModel> GetListKarteFile(int hpId, long ptId, List<long> listRaiinNo, bool isGetAll);

        bool ClearTempData(int hpId, long ptId, List<string> listFileNames);

        long ConvertTextToRichText(int hpId, long ptId);
    }
}
