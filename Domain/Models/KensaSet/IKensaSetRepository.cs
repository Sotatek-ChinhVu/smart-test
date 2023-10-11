using Domain.Common;
using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSetDetail;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {
        public bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted,List<KensaSetDetailModel>kensaSetDetails);
        public List<KensaSetModel> GetListKensaSet(int hpId);
        public List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId);
        public List<KensaCmtMstModel> GetListKensaCmtMst(int hpId, string keyword);
        public bool UpdateKensaInfDetail(int hpId, int userId, List<KensaInfDetailUpdateModel> kensaInfDetails);
        public ListKensaInfDetailModel GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity);
    }
}
