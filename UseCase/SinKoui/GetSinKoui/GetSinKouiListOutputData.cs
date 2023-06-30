using Domain.Models.SinKoui;
using UseCase.Core.Sync.Core;

namespace UseCase.SinKoui.GetSinKoui
{
    public class GetSinKouiListOutputData : IOutputData
    {
        public GetSinKouiListOutputData(GetSinKouiListStatus status, List<string> sinYmBindings)
        {
            Status = status;
            SinYmBindings = sinYmBindings;
        }

        public GetSinKouiListStatus Status { get; private set; }
        public List<string> SinYmBindings { get; private set; } = new List<string>();
    }
}
