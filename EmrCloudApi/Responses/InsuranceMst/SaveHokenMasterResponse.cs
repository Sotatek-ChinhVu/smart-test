using UseCase.InsuranceMst.SaveHokenMaster;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class SaveHokenMasterResponse
    {
        public SaveHokenMasterResponse(SaveHokenMasterStatus state)
        {
            State = state;
        }

        public SaveHokenMasterStatus State { get; private set; }
    }
}
