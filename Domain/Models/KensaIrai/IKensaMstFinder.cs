using Domain.Common;

namespace Domain.Models.KensaIrai
{
    public interface IKensaMstFinder : IRepositoryBase
    {
        List<KensaMstModel> GetParrentKensaMstModels(int hpId, string keyWord);
    }
}
