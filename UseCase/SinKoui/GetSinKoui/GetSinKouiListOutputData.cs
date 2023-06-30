using Domain.Models.SinKoui;
using UseCase.Core.Sync.Core;

namespace UseCase.SinKoui.GetSinKoui
{
    public class GetSinKouiListOutputData : IOutputData
    {
        public GetSinKouiListOutputData(GetSinKouiListStatus status)
        {
            Status = status;
        }

        public GetSinKouiListOutputData(GetSinKouiListStatus status, List<KaikeiInfModel> kaikeiInfs)
        {
            Status = status;
            KaikeiInfs = kaikeiInfs;
        }

        public GetSinKouiListStatus Status { get; private set; }
        public List<KaikeiInfModel> KaikeiInfs { get; private set; } = new List<KaikeiInfModel>();
    }
}
