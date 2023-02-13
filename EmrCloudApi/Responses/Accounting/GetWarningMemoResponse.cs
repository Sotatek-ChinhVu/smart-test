using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetWarningMemoResponse
    {
        public GetWarningMemoResponse(List<WarningMemoModel> warningMemoModels, List<RaiinInfModel> raiinInfModels)
        {
            WarningMemoModels = warningMemoModels;
            RaiinInfModels = raiinInfModels;
        }

        public List<WarningMemoModel> WarningMemoModels { get; private set; }
        public List<RaiinInfModel> RaiinInfModels { get; private set; }
    }
}
