using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetContainerMst;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetContainerMstPresenter : IGetContainerMstOutputPort
    {
        public Response<GetContainerMstResponse> Result { get; private set; } = default!;

        public void Complete(GetContainerMstOutputData outputData)
        {

            Result = new Response<GetContainerMstResponse>()
            {
                Data = new GetContainerMstResponse(outputData.KensaPrinterItems),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetContainerMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetContainerMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetContainerMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
