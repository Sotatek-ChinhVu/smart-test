using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetReceptionDefault
{
    public class GetReceptionDefaultOutputData : IOutputData
    {
        public GetReceptionDefaultOutputData(ReceptionDto data, GetReceptionDefaultStatus status)
        {
            Data = data;
            Status = status;
        }

        public ReceptionDto Data { get; private set; }

        public GetReceptionDefaultStatus Status { get; private set; }
    }
}
