using Domain.Models.KensaSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaSet
{
    public class UpdateKensaSetInputData : IInputData<UpdateKensaSetOuputData>
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public int SetId { get; set; }
        public string SetName { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public List<KensaSetDetailModel> KensaSetDetails { get; set; }

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
