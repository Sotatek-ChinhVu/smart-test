using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf
{
    public class GetSystemConfInputData: IInputData<GetSystemConfOutputData>
    {
        public GetSystemConfInputData(int hpId, int grpCd)
        {
            HpId = hpId;
            GrpCd = grpCd;
        }

        public int HpId { get; private set; }
        public int GrpCd { get; private set; }
    }
}
