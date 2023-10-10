using Domain.Common;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {
        bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted,List<KensaSetDetailModel>kensaSetDetails);
        List<KensaSetModel> GetListKensaSet(int hpId);
        List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
        List<KensaSetModel> GetListKensaSet(int hpId);
    }
}
