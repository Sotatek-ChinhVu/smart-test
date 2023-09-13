using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetParrentKensaMst
{
    public class GetParrentKensaMstOutputData : IOutputData
    {
        public GetParrentKensaMstOutputData(List<KensaMstModel> kensaMsts, GetParrentKensaMstStatus status)
        {
            KensaMsts = kensaMsts;
            Status = status;
        }

        public List<KensaMstModel> KensaMsts { get; private set; }

        public GetParrentKensaMstStatus Status { get; private set; }
    }
}
