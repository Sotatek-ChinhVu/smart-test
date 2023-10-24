using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientManagement;
using UseCase.PatientManagement.GetStaConf;

namespace EmrCloudApi.Presenters.PatientManagement
{
    public class GetStaConfMenuPresenter : IGetStaConfMenuOutputPort
    {
        public Response<GetStaConfMenuResponse> Result { get; private set; } = new Response<GetStaConfMenuResponse>();

        public void Complete(GetStaConfMenuOutputData outputData)
        {
            Result.Data = new GetStaConfMenuResponse(outputData.StatisticMenus, outputData.TenMstItems, outputData.Byomeis);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetStaConfMenuStatus status) => status switch
        {
            GetStaConfMenuStatus.Successed => ResponseMessage.Success,
            GetStaConfMenuStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
