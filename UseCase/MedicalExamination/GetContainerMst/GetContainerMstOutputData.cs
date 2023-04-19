using Domain.Models.Medical;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetContainerMst
{
    public class GetContainerMstOutputData : IOutputData
    {
        public GetContainerMstOutputData(GetContainerMstStatus status, List<KensaPrinterItem> kensaPrinterItems)
        {
            Status = status;
            KensaPrinterItems = kensaPrinterItems;
        }

        public GetContainerMstStatus Status { get; private set; }
        public List<KensaPrinterItem> KensaPrinterItems { get; private set; }
    }
}
