using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDrugAction
{
    public class GetDrugActionInputData : IInputData<GetDrugActionOutputData>
    {
        public GetDrugActionInputData(string yjCd)
        {
            YjCd = yjCd;
        }

        public string YjCd { get; private set; }
    }
}
