using Domain.Models.KensaSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaSet
{
    public class UpdateKensaSetInputData : IInputData<UpdateKensaSetOuputData>
    {
        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int SetId { get; private set; }

        public string SetName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<KensaSetDetailModel> KensaSetDetails { get; private set; }

        public UpdateKensaSetInputData(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted, List<KensaSetDetailModel> kensaSetDetails)
        {
            HpId = hpId;
            UserId = userId;
            SetId = setId;
            SetName = setName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            KensaSetDetails = kensaSetDetails;
        }
    }
}
