
using UseCase.MedicalExamination.OrdInfs.GetListTrees;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination.OrdInfs
{
    public class GetOrdInfListTreeResponse
    {
        public List<GroupHokenItem> GroupHokenItems { get; set; } = new List<GroupHokenItem>();
    }
}
