using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.GetList;

namespace EmrCloudApi.Presenters.InsuranceList
{
    public class GetInsuranceListPresenter : IGetInsuranceListByIdOutputPort
    {
        public Response<GetInsuranceListResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceListByIdOutputData output)
        {
            var convertInsuranceList = output.Data.ListInsurance.Select(i => new PatternDto(i)).ToList();

            var convertHokenInfList = output.Data.ListHokenInf.Select(i => new HokenInfDto(i)).ToList();

            if (output.SortType == 1)
            {
                convertInsuranceList = convertInsuranceList.OrderBy(i => i.IsExpirated).ThenByDescending(i => i.EndDate).ThenByDescending(i => i.HokenId).ToList();
                convertHokenInfList = convertHokenInfList.OrderBy(i => i.IsExpirated).ThenByDescending(h => h.EndDate).ThenByDescending(h => h.HokenId).ToList();
            }
            else if (output.SortType == 2)
            {
                convertInsuranceList = convertInsuranceList.OrderBy(i => i.IsExpirated).ThenBy(i => i.EndDate).ToList();
                convertHokenInfList = convertHokenInfList.OrderBy(h => h.IsExpirated).ThenBy(h => h.EndDate).ToList();
            }

            Result = new Response<GetInsuranceListResponse>()
            {

                Data = new GetInsuranceListResponse()
                {
                    Data = new PatientInsuranceDto(convertInsuranceList, convertHokenInfList, output.Data.ListKohi, output.Data.MaxIdHokenInf, output.Data.MaxIdHokenKohi, output.Data.MaxPidHokenPattern)
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