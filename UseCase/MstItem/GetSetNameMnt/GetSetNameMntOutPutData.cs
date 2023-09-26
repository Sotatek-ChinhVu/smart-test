using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSetNameMnt
{
    public sealed class GetSetNameMntOutPutData :IOutputData
    {
        public GetSetNameMntOutPutData(List<SetNameMntModel> setNameMnts, GetSetNameMntStatus status)
        {
            SetNameMnts = setNameMnts;
            Status = status;
        }

        public List<SetNameMntModel> SetNameMnts { get; private set; }
        public GetSetNameMntStatus Status { get; private set; }
    }
}
