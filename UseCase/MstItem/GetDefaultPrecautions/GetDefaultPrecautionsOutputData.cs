using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDefaultPrecautions
{
    public class GetDefaultPrecautionsOutputData : IOutputData
    {
        public GetDefaultPrecautionsOutputData(string drugInfo, GetDefaultPrecautionsStatus status)
        {
            DrugInfo = drugInfo;
            Status = status;
        }

        public string DrugInfo { get; private set; }

        public GetDefaultPrecautionsStatus Status { get; private set; }
    }
}
