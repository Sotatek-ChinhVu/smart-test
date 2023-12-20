using Domain.Common;

namespace Domain.Models.ReleasenoteRead;

public interface IReleasenoteReadRepository : IRepositoryBase
{
    List<string> GetListReleasenote(int hpId, int userId);
}
