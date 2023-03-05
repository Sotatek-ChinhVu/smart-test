using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetSinMei
{
    public class GetMeiHoGaiOutputData : IOutputData
    {
        public GetMeiHoGaiOutputData(List<SinMeiModel> sinMeiModels, List<SinHoModel> sinHoModels, List<SinGaiModel> sinGaiModels, GetMeiHoGaiStatus status)
        {
            SinMeiModels = sinMeiModels;
            SinHoModels = sinHoModels;
            SinGaiModels = sinGaiModels;
            Status = status;
        }

        public List<SinMeiModel> SinMeiModels { get; private set; }
        public List<SinHoModel> SinHoModels { get; private set; }
        public List<SinGaiModel> SinGaiModels { get; private set; }
        public GetMeiHoGaiStatus Status { get; private set; }
    }
}
