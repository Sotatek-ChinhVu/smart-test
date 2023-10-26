using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.GetListQualification;

namespace EmrCloudApi.Presenters.MainMenu
{
    public class GetListQualificationInfPresenter : IGetListQualificationInfOutputPort
    {
        public Response<GetListQualificationInfResponse> Result { get; private set; } = new();
        public void Complete(GetListQualificationInfOutputData outputData)
        {
            Result.Data = new GetListQualificationInfResponse(outputData.QualificationInfs);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetListQualificationInfStatus status) => status switch
        {
            GetListQualificationInfStatus.Successed => ResponseMessage.Success,
            GetListQualificationInfStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
