using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf
{
    public class GetSystemGenerationConfOutputData : IOutputData
    {
        public GetSystemGenerationConfOutputData(int value, string param,  GetSystemGenerationConfStatus status)
        {
            Value = value;
            Param = param;
            Status = status;
        }

        public int Value { get; private set; }

        public string Param { get; private set; }

        public GetSystemGenerationConfStatus Status { get; private set; }
    }
}
