using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor.PtKyuseiInf;
using UseCase.PatientInfor.PtKyuseiInf.GetList;

namespace EmrCloudApi.Presenters.PatientInfor.PtKyusei
{
    public class GetPtKyuseiInfPresenter : IGetPtKyuseiInfOutputPort
    {
        public Response<GetPtKyuseiInfResponse> Result { get; private set; } = new Response<GetPtKyuseiInfResponse>();

        public void Complete(GetPtKyuseiInfOutputData outputData)
        {
            Result.Data = new GetPtKyuseiInfResponse(outputData.PtKyuseiInfModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetPtKyuseiInfStatus status) => status switch
        {
            GetPtKyuseiInfStatus.Success => ResponseMessage.Success,
            GetPtKyuseiInfStatus.Failed => ResponseMessage.Failed,
            GetPtKyuseiInfStatus.NoData => ResponseMessage.NoData,
            GetPtKyuseiInfStatus.InValidHpId => ResponseMessage.InvalidHpId,
            GetPtKyuseiInfStatus.InValidPtId => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
