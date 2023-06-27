using ContainerOdrInfItem = UseCase.MedicalExamination.GetContainerMst.OdrInfItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetContainerMstRequest
    {
        public int SinDate { get; set; }

        public bool DefaultChecked { get; set; }

        public List<ContainerOdrInfItem> OdrInfItems { get; set; } = new();
    }
}
