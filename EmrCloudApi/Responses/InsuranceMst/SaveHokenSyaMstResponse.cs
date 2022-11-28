using UseCase.InsuranceMst.SaveHokenSyaMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class SaveHokenSyaMstResponse
    {
        public SaveHokenSyaMstResponse(SaveHokenSyaMstStatus state)
        {
            State = state;
        }

        public SaveHokenSyaMstStatus State { get; private set; }
    }
}
