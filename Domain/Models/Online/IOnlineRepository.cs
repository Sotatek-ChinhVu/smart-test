using Domain.Common;

namespace Domain.Models.Online;

public interface IOnlineRepository : IRepositoryBase
{
    bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList);
}
