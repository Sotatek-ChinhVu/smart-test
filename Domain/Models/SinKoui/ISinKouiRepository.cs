using Domain.Common;
using Domain.Models.User;

namespace Domain.Models.SinKoui
{
    public interface ISinKouiRepository : IRepositoryBase
    {
        List<KaikeiInfModel> GetListKaikeiInf(int hpId, long ptId);
    }
}
