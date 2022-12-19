using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetHokenSyaMst
{
    public class GetHokenSyaMstOutputData : IOutputData
    {
        public GetHokenSyaMstOutputData(HokensyaMstModel data, GetHokenSyaMstStatus status)
        {
            Data = data;
            Status = status;
        }

        public HokensyaMstModel Data { get; private set; }

        public GetHokenSyaMstStatus Status { get; private set; }
    }
}
