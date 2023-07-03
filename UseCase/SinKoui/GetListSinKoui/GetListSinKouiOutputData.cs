using UseCase.Core.Sync.Core;

namespace UseCase.SinKoui.GetSinKoui
{
    public class GetListSinKouiOutputData : IOutputData
    {
        public GetListSinKouiOutputData(GetListSinKouiStatus status, List<int> sinYms)
        {
            Status = status;
            SinYms = sinYms;
        }

        public GetListSinKouiStatus Status { get; private set; }
        public List<int> SinYms { get; private set; } = new List<int>();
    }
}
