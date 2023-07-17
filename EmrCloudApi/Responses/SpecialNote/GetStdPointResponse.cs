using Domain.Models.SpecialNote.PatientInfo;

namespace EmrCloudApi.Responses.SpecialNote;

public class GetStdPointResponse
{
    public GetStdPointResponse(List<GcStdInfModel> gcStdInfModels)
    {
        GcStdInfModels = gcStdInfModels;
    }

    public List<GcStdInfModel> GcStdInfModels { get; private set; }
}
