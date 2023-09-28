using Domain.Common;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {
        public bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted,List<KensaSetDetailModel>kensaSetDetails);
        public List<KensaSetModel> GetListKensaSet(int hpId);
        public List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
    }
}
