using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDefaultPrecautions
{
    public class GetDefaultPrecautionsInputData : IInputData<GetDefaultPrecautionsOutputData>
    {
        public GetDefaultPrecautionsInputData(string yjCd)
        {
            YjCd = yjCd;
        }

        public string YjCd { get; private set; }
    }
}
