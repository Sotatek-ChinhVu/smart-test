using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetKensaCenterMsts
{
    public class GetKensaCenterMstsOutputData : IOutputData
    {
        public GetKensaCenterMstsOutputData(Dictionary<string, string> kensaCenterMsts, GetKensaCenterMstsStatus status) 
        {
            KensaCenterMsts = kensaCenterMsts;
            Status = status;
        }

        public Dictionary<string, string> KensaCenterMsts { get; private set; }

        public GetKensaCenterMstsStatus Status { get; private set; }
    }
}
