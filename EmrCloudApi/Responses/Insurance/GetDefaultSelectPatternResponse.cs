using UseCase.Insurance.GetDefaultSelectPattern;

namespace EmrCloudApi.Responses.InsuranceList
{
    public class GetDefaultSelectPatternResponse
    {
        public GetDefaultSelectPatternResponse(List<GetDefaultSelectPatternItem> hokenPids)
        {
            HokenPids = hokenPids;
        }

        public List<GetDefaultSelectPatternItem> HokenPids { get; private set; }
    }
}