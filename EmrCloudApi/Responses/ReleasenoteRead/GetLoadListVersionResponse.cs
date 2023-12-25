using Domain.Models.ReleasenoteRead;
using UseCase.Releasenote.LoadListVersion;

namespace EmrCloudApi.Responses.ReleasenoteRead
{
    public class GetLoadListVersionResponse
    {
        public GetLoadListVersionResponse(List<ReleasenoteReadModel> fileName_Path, GetLoadListVersionStatus status)
        {
            FileName_Path = fileName_Path;
            Status = status;
        }

        public List<ReleasenoteReadModel> FileName_Path { get; private set; }

        public GetLoadListVersionStatus Status { get; private set; }
    }
}
