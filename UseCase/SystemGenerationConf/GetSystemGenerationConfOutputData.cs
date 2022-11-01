using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf
{
    public class GetSystemGenerationConfOutputData : IOutputData
    {
        public GetSystemGenerationConfOutputData(int value, GetSystemGenerationConfStatus status)
        {
            Value = value;
            Status = status;
        }

        public int Value { get; private set; }

        public GetSystemGenerationConfStatus Status { get; private set; }
    }
}
