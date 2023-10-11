using Domain.Common;
using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {

        bool UpdateKensaInfDetail(int hpId, int userId, List<KensaInfDetailUpdateModel> kensaInfDetails);
        List<KensaCmtMstModel> GetListKensaCmtMst(int hpId, string keyword);
        bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted,List<KensaSetDetailModel>kensaSetDetails);
        List<KensaSetModel> GetListKensaSet(int hpId);
        List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
    }
}
