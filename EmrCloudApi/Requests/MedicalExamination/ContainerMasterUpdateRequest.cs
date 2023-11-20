using Helper.Constants;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ContainerMasterUpdateRequest
    {
        public List<ContainerMasterRequest> ContainerMasterList { get; set; } = new List<ContainerMasterRequest>();
    }

    public class ContainerMasterRequest
    {
        public long ContainerCd { get; set; }

        public string ContainerName { get; set; } = string.Empty;

        public ModelStatus ContainerModelStatus { get; set; }
    }
}
