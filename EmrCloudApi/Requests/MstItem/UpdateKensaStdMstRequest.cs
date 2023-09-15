using UseCase.MstItem.UpdateKensaStdMst;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateKensaStdMstRequest
    {
        public List<UpdateKensaStdMstInputItem> KensaMstItems { get; set; } = new();
    }
}
