using Domain.Models.SpecialNote.PatientInfo;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.GetStdPoint;

public class GetStdPointOutputData : IOutputData
{
    public GetStdPointOutputData(List<GcStdInfModel> gcStdInfModels, GetStdPointStatus status)
    {
        Status = status;
        GcStdInfModels = gcStdInfModels;
    }

    public List<GcStdInfModel> GcStdInfModels { get; private set; }

    public GetStdPointStatus Status { get; private set; }
}
