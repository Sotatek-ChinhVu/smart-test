using Domain.Common;

namespace Domain.Models.Online;

public interface IOnlineRepository : IRepositoryBase
{
    bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList);

    List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1);

    bool UpdateOnlineConfirmationHistory(int uketukeStatus, int id, int userId);

    bool UpdateOnlineHistoryById(int userId, long id, long ptId, int uketukeStatus, int confirmationType);

    bool CheckExistIdList(List<long> idList);
}
