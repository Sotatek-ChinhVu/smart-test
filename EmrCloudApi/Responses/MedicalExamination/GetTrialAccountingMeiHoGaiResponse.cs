using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiResponse
    {
        public GetTrialAccountingMeiHoGaiResponse(List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais)
        {
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
        }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }
    }
}
