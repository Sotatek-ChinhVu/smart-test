using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenItemCds
{
    public class GetTenItemCdsOutputData : IOutputData
    {
        public GetTenItemCdsOutputData(List<string> datas, GetTenItemCdsStatus status)
        {
            Datas = datas;
            Status = status;
        }

        public List<string> Datas { get; private set; } = new();

        public GetTenItemCdsStatus Status { get; private set; }
    }
}
