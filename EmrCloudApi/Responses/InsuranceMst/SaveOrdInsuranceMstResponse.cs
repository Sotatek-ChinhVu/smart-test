using UseCase.InsuranceMst.SaveOrdInsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class SaveOrdInsuranceMstResponse
    {
        public SaveOrdInsuranceMstResponse(SaveOrdInsuranceMstStatus state)
        {
            State = state;
        }

        public SaveOrdInsuranceMstStatus State { get; private set; }
    }
}
