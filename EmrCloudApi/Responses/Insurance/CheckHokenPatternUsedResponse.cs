using UseCase.Insurance.HokenPatternUsed;

namespace EmrCloudApi.Responses.Insurance
{
    public class CheckHokenPatternUsedResponse
    {
        public CheckHokenPatternUsedResponse(bool hokenPatternIsUsed , HokenPatternUsedStatus status )
        {
            HokenPatternIsUsed = hokenPatternIsUsed;
            Status = status;
        }

        public bool HokenPatternIsUsed { get; private set; }

        public HokenPatternUsedStatus Status { get;private set; }
    }
}
