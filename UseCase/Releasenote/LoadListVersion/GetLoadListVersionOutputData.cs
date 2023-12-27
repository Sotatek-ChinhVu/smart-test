using Domain.Models.ReleasenoteRead;
using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.LoadListVersion
{
    public class GetLoadListVersionOutputData : IOutputData
    {
        public GetLoadListVersionOutputData(List<ReleasenoteReadModel> data, bool showReleaseNote, GetLoadListVersionStatus status)
        {
            Data = data;
            Status = status;
            ShowReleaseNote = showReleaseNote;
        }
        public GetLoadListVersionStatus Status { get; private set; }

        public bool ShowReleaseNote { get; private set; }

        public List<ReleasenoteReadModel> Data { get; private set; }
    }
}
