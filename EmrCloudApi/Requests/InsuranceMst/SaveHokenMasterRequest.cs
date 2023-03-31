using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Requests.InsuranceMst
{
    public class SaveHokenMasterRequest
    {
        public SaveHokenMasterRequest(HokenMasterDto insurance, List<ExceptHokensyaModel> excepHokenSyas)
        {
            Insurance = insurance;
            ExcepHokenSyas = excepHokenSyas;
        }

        public HokenMasterDto Insurance { get; private set; }

        public List<ExceptHokensyaModel> ExcepHokenSyas { get; private set; } = new List<ExceptHokensyaModel>();
    }
}
