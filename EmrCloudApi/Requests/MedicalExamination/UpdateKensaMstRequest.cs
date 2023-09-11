using Domain.Models.KensaIrai;
using Domain.Models.TenMst;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class UpdateKensaMstRequest
    {
        public List<KensaMstItem> KensaMstItems { get; set; } = new();

        public List<TenMstItemModel> TenMstItems { get; set; } = new();
    }
}
