using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetKacodeYousikiMstDict
{
    public class GetKacodeYousikiMstDictOutputData : IOutputData
    {
        public GetKacodeYousikiMstDictOutputData(Dictionary<string, string> kacodeYousikiMstDict, GetKacodeYousikiMstDictStatus status)
        {
            KacodeYousikiMstDict = kacodeYousikiMstDict;
            Status = status;
        }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

        public GetKacodeYousikiMstDictStatus Status { get; private set; }
    }
}
