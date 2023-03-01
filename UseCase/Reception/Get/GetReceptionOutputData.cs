using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Get
{
    public class GetReceptionOutputData : IOutputData
    {
        public ReceptionDto ReceptionDto { get; private set; }

        public GetReceptionStatus Status { get; private set; }

        public GetReceptionOutputData(ReceptionDto receptionDto, GetReceptionStatus status)
        {
            ReceptionDto = receptionDto;
            Status = status;
        }
    }
}
