using Domain.Common;

namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository : IRepositoryBase
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int userId, int setCd, int sindate);

    long GetLastSeqNo(int hpId, int setCd);

    int SaveSuperSetDetail(int setCd, int userId, int hpId, List<SetByomeiModel> SetByomeiList, SetKarteInfModel SetKarteInf, List<SetOrderInfModel> ListSetOrdInfModels);

    bool SaveListSetKarteFile(int hpId, int setCd, string host, List<SetFileInfModel> listFiles, bool saveTempFile);

    List<SetOrderInfModel> GetOnlyListOrderInfModel(int hpId, int setCd);

    (List<SetByomeiModel> byomeis, List<SetKarteInfModel> karteInfs, List<SetOrderInfModel>) GetSuperSetDetailForTodayOrder(int hpId, int userId, int setCd, int sinDate);

    bool CheckExistSupperSetDetail(int hpId, int setCd);

    bool ClearTempData(int hpId, List<string> listFileNames);
}