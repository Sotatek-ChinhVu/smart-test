using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class GetTrialAccountingMeiHoGaiOutputData : IOutputData
    {
        public GetTrialAccountingMeiHoGaiOutputData(List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais, GetTrialAccountingMeiHoGaiStatus status)
        {
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
            Status = status;
        }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }

        public GetTrialAccountingMeiHoGaiStatus Status { get; private set; }
    }
}
