using Domain.Models.ReleasenoteRead;
using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.LoadListVersion
{
    public class GetLoadListVersionOutputData : IOutputData
    {
        public GetLoadListVersionOutputData(List<ReleasenoteReadModel> data, GetLoadListVersionStatus status)
        {
            Data = data;
            Status = status;
        }
        public GetLoadListVersionStatus Status { get; private set; }

        public List<ReleasenoteReadModel> Data { get; private set; }
    }
}
