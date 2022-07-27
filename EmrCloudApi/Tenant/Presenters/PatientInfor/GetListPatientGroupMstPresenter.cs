using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientGroupMst.GetList;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor
{
    public class GetListPatientGroupMstPresenter : IGetListPatientGroupMstOutputPort
    {
        public Response<GetListPatientGroupMstResponse> Result { get; private set; } = default!;

        public void Complete(GetListPatientGroupMstOutputData outputData)
        {
            Result = new Response<GetListPatientGroupMstResponse>()
            {
                Status = 1,
                Message = ResponseMessage.Success,
                Data = new GetListPatientGroupMstResponse(outputData.PatientGroupMstModels)
            };
        }
    }
}
