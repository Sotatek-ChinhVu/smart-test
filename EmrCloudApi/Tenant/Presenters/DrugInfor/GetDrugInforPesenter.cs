using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugInfor;
using UseCase.DrugInfor.Get;

namespace EmrCloudApi.Tenant.Presenters.DrugInfor
{
    public class GetDrugInforPresenter : IGetDrugInforOutputPort
    {
        public Response<GetDrugInforResponse> Result { get; private set; } = new Response<GetDrugInforResponse>();

        public void Complete(GetDrugInforOutputData outputData)
        {
            Result = new Response<GetDrugInforResponse>()
            {
                Data = new GetDrugInforResponse(outputData.DrugInfor),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetDrugInforStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetDrugInforStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetDrugInforStatus.InValidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetDrugInforStatus.InValidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
            }
        }
    }
}
