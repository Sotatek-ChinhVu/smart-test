using Domain.Common;
using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {

        ListKensaInfDetailModel GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int itemQuantity, List<long> listSeqNoItems, int startDate, int endDate);
        bool UpdateKensaInfDetail(int hpId, int userId, int ptId, long iraiCd, int iraiDate, List<KensaInfDetailUpdateModel> kensaInfDetails);
        List<KensaCmtMstModel> GetListKensaCmtMst(int hpId, int iraiCd, string keyword);
        bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted, List<KensaSetDetailModel> kensaSetDetails);
        List<KensaSetModel> GetListKensaSet(int hpId);
        List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
        List<ListKensaInfDetailItemModel> GetKensaInfDetailByIraiCd(int hpId, int iraiCd);
    }
}
