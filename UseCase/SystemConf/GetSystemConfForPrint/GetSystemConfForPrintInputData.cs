using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConfForPrint
{
    public class GetSystemConfForPrintInputData : IInputData<GetSystemConfForPrintOutputData>
    {
        public GetSystemConfForPrintInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
