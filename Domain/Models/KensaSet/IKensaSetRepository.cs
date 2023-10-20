using Domain.Common;
using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {

        ListKensaInfDetailModel GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity);
        bool UpdateKensaInfDetail(int hpId, int userId, List<KensaInfDetailUpdateModel> kensaInfDetails);
        List<KensaCmtMstModel> GetListKensaCmtMst(int hpId, string keyword);
        bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted,List<KensaSetDetailModel>kensaSetDetails);
        List<KensaSetModel> GetListKensaSet(int hpId);
        List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
    }
}
