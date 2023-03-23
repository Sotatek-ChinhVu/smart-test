using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceByomeiChecking;

public class GetReceByomeiCheckingOutputData : IOutputData
{
    public GetReceByomeiCheckingOutputData(GetReceByomeiCheckingStatus status, Dictionary<OrdInfDetailModel, List<PtDiseaseModel>> data)
    {
        Status = status;
        Data = data;
    }

    public GetReceByomeiCheckingStatus Status { get; private set; }

    public Dictionary<OrdInfDetailModel, List<PtDiseaseModel>> Data { get; private set; }
}
