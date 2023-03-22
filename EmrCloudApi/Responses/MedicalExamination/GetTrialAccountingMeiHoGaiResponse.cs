using Domain.Models.Accounting;
using UseCase.MedicalExamination.TrailAccounting;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiResponse
    {
        public GetTrialAccountingMeiHoGaiResponse(List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais, TrialAccountingInfDto accountingInf, string hokenPatternRate)
        {
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
            AccountingInf = accountingInf;
            HokenPatternRate = hokenPatternRate;
        }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }

        public TrialAccountingInfDto AccountingInf { get; private set; }

        public string HokenPatternRate { get; private set; }
    }
}
