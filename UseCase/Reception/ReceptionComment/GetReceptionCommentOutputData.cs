using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.ReceptionComment
{
    public class GetReceptionCommentOutputData : IOutputData
    {
        public ReceptionDto ReceptionDto { get; private set; }

        public GetReceptionCommentStatus Status { get; private set; }

        public GetReceptionCommentOutputData(ReceptionDto receptionDto, GetReceptionCommentStatus status)
        {
            ReceptionDto = receptionDto;
            Status = status;
        }

        public GetReceptionCommentOutputData(GetReceptionCommentStatus status)
        {
            ReceptionDto = new ReceptionDto();
            Status = status;
        }
    }
}
