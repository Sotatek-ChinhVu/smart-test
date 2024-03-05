using Domain.Common;
using Domain.Models.Online.QualificationConfirmation;
using Domain.Models.Reception;
using Helper.Constants;

namespace Domain.Models.Online;

public interface IOnlineRepository : IRepositoryBase
{
    List<long> InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList);

    List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1);

    bool UpdateOnlineConfirmationHistory(int uketukeStatus, int id, int userId);

    bool UpdateOnlineHistoryById(int userId, long id, long ptId, int uketukeStatus, int confirmationType);

    bool CheckExistIdList(List<long> idList);

    bool UpdateOQConfirmation(int hpId, int userId, long onlineHistoryId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg, int prescriptionIssueType)> onlQuaConfirmationTypeDict);

    bool SaveAllOQConfirmation(int hpId, int userId, long ptId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict);

    bool SaveOQConfirmation(int hpId, int userId, long onlineHistoryId, long ptId, string confirmationResult, string onlineConfirmationDateString, int confirmationType, string infConsFlg, int uketukeStatus = 0, bool isUpdateRaiinInf = true);

    bool UpdateOnlineInRaiinInf(int hpId, int userId, long ptId, DateTime onlineConfirmationDate, int confirmationType, string infConsFlg);

    long UpdateRefNo(int hpId, long ptId);

    bool UpdatePtInfOnlineQualify(int hpId, int userId, long ptId, List<PtInfConfirmationModel> resultList);

    List<OnlineConfirmationHistoryModel> GetListOnlineConfirmationHistoryModel(long ptId);

    List<OnlineConfirmationHistoryModel> GetListOnlineConfirmationHistoryModel(int userId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict);

    List<OnlineConsentModel> GetOnlineConsentModel(long ptId);

    bool UpdateOnlineConsents(int userId, long ptId, List<QCXmlMsgResponse> responseList);

    List<QualificationInfModel> GetListQualificationInf();

    bool SaveOnlineConfirmation(int userId, QualificationInfModel qualificationInf, ModelStatus status);

    bool InsertListOnlConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> listOnlineConfirmationHistoryModel);

    (bool, List<ReceptionRowModel> receptions) UpdateRaiinInfByResResult(int hpId, int userId, List<ConfirmResultModel> listResResult);

    bool ExistOnlineConsent(long ptId, int sinDate);
}
