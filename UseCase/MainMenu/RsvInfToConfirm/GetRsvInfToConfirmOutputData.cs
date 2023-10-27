using Domain.Models.RsvInf;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.RsvInfToConfirm
{
    public class GetRsvInfToConfirmOutputData : IOutputData
    {
        public GetRsvInfToConfirmOutputData(List<RsvInfToConfirmModel> rsvInfToConfirms, GetRsvInfToConfirmStatus status)
        {
            RsvInfToConfirms = rsvInfToConfirms;
            Status = status;
        }

        public List<RsvInfToConfirmModel> RsvInfToConfirms { get; private set; }

        public GetRsvInfToConfirmStatus Status { get; private set; }
    }
}
