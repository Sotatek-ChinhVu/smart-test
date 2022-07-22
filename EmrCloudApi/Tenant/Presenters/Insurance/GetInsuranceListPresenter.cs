using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using Microsoft.AspNetCore.Mvc;
using UseCase.Insurance.GetList;

namespace EmrCloudApi.Tenant.Presenters.InsuranceList
{
    public class GetInsuranceListPresenter : IGetInsuranceListByIdOutputPort
    {
        public Response<GetInsuranceListResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceListByIdOutputData output)
        {
            Result = new Response<GetInsuranceListResponse>()
            {

                Data = new GetInsuranceListResponse()
                {
                    ListData = output.ListData
                },
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                
                case GetInsuranceListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidPtId;
                    break;
                case GetInsuranceListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidHpId;
                    break;
                case GetInsuranceListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidSinDate;
                    break;
                case GetInsuranceListStatus.Successed:
                    Result.Message = ResponseMessage.GetInsuranceListSuccessed;
                    break;
            }
        }
    }
}