using Domain.Common;

namespace Domain.Models.Online;

public interface IOnlineRepository : IRepositoryBase
{
    bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList);

    List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1);
}
