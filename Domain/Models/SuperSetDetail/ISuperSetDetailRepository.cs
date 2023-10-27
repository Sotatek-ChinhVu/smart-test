using Domain.Common;
using Domain.Models.SetMst;

namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository : IRepositoryBase
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int userId, int setCd, int sindate);

    long GetLastSeqNo(int hpId, int setCd);

    int SaveSuperSetDetail(int setCd, int userId, int hpId, List<SetByomeiModel> SetByomeiList, SetKarteInfModel SetKarteInf, List<SetOrderInfModel> ListSetOrdInfModels);

    bool SaveListSetKarteFile(int hpId, int setCd, string host, List<SetFileInfModel> listFiles, bool saveTempFile);

    List<SetOrderInfModel> GetOnlyListOrderInfModel(int hpId, int setCd);

    (List<SetByomeiModel> byomeis, List<SetKarteInfModel> karteInfs, List<SetOrderInfModel> orderInfModels, List<(int setCd, List<SetFileInfModel> setFiles)> setFileInfModels) GetSuperSetDetailForTodayOrder(int hpId, int userId, int setCd, int sinDate);

    bool CheckExistSupperSetDetail(int hpId, int setCd);

    bool ClearTempData(int hpId, List<string> listFileNames);

    List<ConversionItemInfModel> GetConversionItem(int hpId, string itemCd, int sinDate);

    bool SaveConversionItemInf(int hpId, int userId, string conversionItemCd, string sourceItemCd, List<string> deleteConversionItemCdList);

    List<OdrSetNameModel> GetOdrSetName(int hpId, SetCheckBoxStatusModel checkBoxStatus, int generationId, int timeExpired, string itemName);

    (bool SaveSuccess, List<SetMstModel> SetMstUpdateList) SaveOdrSet(int hpId, int userId, int sinDate, List<OdrSetNameModel> setNameModelList, List<OdrSetNameModel> updateSetNameList);
}