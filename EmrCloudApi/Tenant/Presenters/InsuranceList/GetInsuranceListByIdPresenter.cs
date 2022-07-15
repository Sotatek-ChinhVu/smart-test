using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using Microsoft.AspNetCore.Mvc;
using UseCase.InsuranceList.GetInsuranceListById;

namespace EmrCloudApi.Tenant.Presenters.InsuranceList
{
    public class GetInsuranceListByIdPresenter : IGetInsuranceListByIdOutputPort
    {
        public Response<GetInsuranceListByIdResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceListByIdOutputData output)
        {
            Result = new Response<GetInsuranceListByIdResponse>()
            {

                Data = new GetInsuranceListByIdResponse()
                {
                    Data = output.ListData
                },
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case GetInsuranceListByIdStatus.InvalidId:
                    Result.Message = ResponseMessage.GetInsuranceListByPtIdInvalidId;
                    break;
                case GetInsuranceListByIdStatus.Successed:
                    Result.Message = ResponseMessage.GetInsuranceListByPtIdSuccessed;
                    break;
                case GetInsuranceListByIdStatus.DataNotExist:
                    Result.Message = ResponseMessage.DataNotExist;
                    break;
            }
        }
    }
}
