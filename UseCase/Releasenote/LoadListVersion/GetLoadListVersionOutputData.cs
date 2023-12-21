using Domain.Models.ReleasenoteRead;
using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.LoadListVersion
{
    public class GetLoadListVersionOutputData : IOutputData
    {
        public GetLoadListVersionOutputData(Task<List<ReleasenoteReadModel>> data, GetLoadListVersionStatus status)
        {
            Data = data;
            Status = status;
        }
        public GetLoadListVersionStatus Status { get; private set; }

        public Task<List<ReleasenoteReadModel>> Data { get; private set; }
    }
}
