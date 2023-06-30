using Domain.Common;
using Domain.Models.User;

namespace Domain.Models.SinKoui
{
    public interface ISinKouiRepository : IRepositoryBase
    {
        List<string> GetListKaikeiInf(int hpId, long ptId);
    }
}
