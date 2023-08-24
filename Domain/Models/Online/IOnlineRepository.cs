using Domain.Common;

namespace Domain.Models.Online;

public interface IOnlineRepository : IRepositoryBase
{
    bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList);

    List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1);

    bool UpdateOnlineConfirmationHistory(int uketukeStatus, int id, int userId);

    bool UpdateOnlineHistoryById(int userId, long id, long ptId, int uketukeStatus, int confirmationType);

    bool CheckExistIdList(List<long> idList);

    bool UpdateOQConfirmation(int hpId, int userId, long onlineHistoryId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict);

    bool SaveAllOQConfirmation(int hpId, int userId, long ptId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> OnlQuaConfirmationTypeDict);

    bool SaveOQConfirmation(int hpId, int userId, long onlineHistoryId, long ptId, string confirmationResult, string onlineConfirmationDateString, int confirmationType, string infConsFlg, int uketukeStatus = 0);
}
