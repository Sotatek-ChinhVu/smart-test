using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDrugAction
{
    public class GetDrugActionOutputData : IOutputData
    {
        public GetDrugActionOutputData(string drugInfo, GetDrugActionStatus status)
        {
            DrugInfo = drugInfo;
            Status = status;
        }

        public string DrugInfo { get; private set; }
        public GetDrugActionStatus Status { get; private set; }
    }
}
