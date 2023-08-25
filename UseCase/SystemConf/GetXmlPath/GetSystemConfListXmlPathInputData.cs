using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetXmlPath
{
    public class GetSystemConfListXmlPathInputData : IInputData<GetSystemConfListXmlPathOutputData>
    {
        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public string Machine { get; private set; }

        public GetSystemConfListXmlPathInputData(int hpId, int grpCd, string machine)
        {
            HpId = hpId;
            GrpCd = grpCd;
            Machine = machine;
        }
    }
}