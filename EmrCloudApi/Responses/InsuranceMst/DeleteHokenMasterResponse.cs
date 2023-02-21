using UseCase.InsuranceMst.DeleteHokenMaster;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class DeleteHokenMasterResponse
    {
        public DeleteHokenMasterResponse(DeleteHokenMasterStatus state)
        {
            State = state;
        }

        public DeleteHokenMasterStatus State { get; private set; }
    }
}
