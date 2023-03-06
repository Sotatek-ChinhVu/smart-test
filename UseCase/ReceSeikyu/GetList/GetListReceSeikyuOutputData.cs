using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetList
{
    public class GetListReceSeikyuOutputData : IOutputData
    {
        public GetListReceSeikyuOutputData(GetListReceSeikyuStatus status, List<ReceSeikyuModel> datas)
        {
            Status = status;
            Datas = datas;
        }

        public GetListReceSeikyuStatus Status { get; private set; }

        public List<ReceSeikyuModel> Datas { get; private set; }
    }
}
