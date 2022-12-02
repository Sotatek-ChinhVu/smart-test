using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Responses.OrdInfs
{
    public class GetOrdInfListTreeResponse
    {
        public List<GroupHokenItem> GroupHokenItems { get; set; } = new List<GroupHokenItem>();
    }
}
