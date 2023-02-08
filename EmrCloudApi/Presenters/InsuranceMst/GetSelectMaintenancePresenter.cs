using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using UseCase.InsuranceMst.GetSelectMaintenance;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class GetSelectMaintenancePresenter : IGetSelectMaintenanceOutputPort
    {
        public Response<GetSelectMaintenanceResponse> Result { get; private set; } = default!;

        public void Complete(GetSelectMaintenanceOutputData output)
        {
            Result = new Response<GetSelectMaintenanceResponse>()
            {
                Data = new GetSelectMaintenanceResponse(output.Datas),
                Status = (int)output.Status,
            };
            switch (output.Status)
            {
                case GetSelectMaintenanceStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetSelectMaintenanceStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetSelectMaintenanceStatus.InvalidHokenEdaNo:
                    Result.Message = ResponseMessage.InvalidHokenEdaNo;
                    break;

                case GetSelectMaintenanceStatus.InvalidHokenNo:
                    Result.Message = ResponseMessage.InvalidHokenNo;
                    break;

                case GetSelectMaintenanceStatus.InvalidPrefNo:
                    Result.Message = ResponseMessage.InvalidPrefNo;
                    break;

                case GetSelectMaintenanceStatus.InvalidStartDate:
                    Result.Message = ResponseMessage.InvalidStartDate;
                    break;

                case GetSelectMaintenanceStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}