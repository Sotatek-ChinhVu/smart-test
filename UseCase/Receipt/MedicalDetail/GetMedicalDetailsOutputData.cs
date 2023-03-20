using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.MedicalDetail
{
    public class GetMedicalDetailsOutputData : IOutputData
    {
        public GetMedicalDetailsOutputData(List<SinMeiModel> sinMeiModels, Dictionary<int, string> holidays, GetMedicalDetailsStatus status)
        {
            SinMeiModels = sinMeiModels;
            Holidays = holidays;
            Status = status;
        }

        public List<SinMeiModel> SinMeiModels { get; private set; }
        public Dictionary<int, string> Holidays { get; private set; }
        public GetMedicalDetailsStatus Status { get; private set; }
    }
}
