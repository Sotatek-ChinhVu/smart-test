using UseCase.UpdateKensaMst;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class UpdateKensaMstRequest
    {
        public List<KensaMstInputItem> KensaMstItems { get; set; } = new();

        public List<ChildKensaMstInputItem> ChildKensaMstItems { get; set; } = new();

        public List<TenMstInputItem> TenMstItems { get; set; } = new();
    }
}
