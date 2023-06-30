using Domain.Models.SinKoui;
using UseCase.Core.Sync.Core;

namespace UseCase.SinKoui.GetSinKoui
{
    public class GetListSinKouiOutputData : IOutputData
    {
        public GetListSinKouiOutputData(GetListSinKouiStatus status, List<string> sinYmBindings)
        {
            Status = status;
            SinYmBindings = sinYmBindings;
        }

        public GetListSinKouiStatus Status { get; private set; }
        public List<string> SinYmBindings { get; private set; } = new List<string>();
    }
}
