﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
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
                    Data = new PatientInsuranceDto(output.Data.ListInsurance, output.Data.ListHokenInf, output.Data.ListKohi)
                },
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case GetInsuranceListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetInsuranceListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetInsuranceListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetInsuranceListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}