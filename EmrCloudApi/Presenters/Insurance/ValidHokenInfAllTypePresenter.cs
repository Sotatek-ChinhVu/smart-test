using EmrCloudApi.Responses.Insurance;
using EmrCloudApi.Responses;
using UseCase.Insurance.ValidHokenInfAllType;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.Insurance
{
    public class ValidHokenInfAllTypePresenter : IValidHokenInfAllTypeOutputPort
    {
        public Response<ValidHokenInfAllTypeResponse> Result { get; private set; } = default!;

        public void Complete(ValidHokenInfAllTypeOutputData outputData)
        {
            outputData.ValidateDetails.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Message))
                {
                    switch (x.Status)
                    {
                        case ValidHokenInfAllTypeStatus.InvalidSuccess:
                            x.Message = ResponseMessage.Success;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidHpId:
                            x.Message = ResponseMessage.InvalidHpId;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSinDate:
                            x.Message = ResponseMessage.InvalidSinDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidPtBirthday:
                            x.Message = ResponseMessage.InvalidPtBirthday;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfStartDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfEndDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRousaiSaigaiKbn:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSaigaiKbn;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRousaiSyobyoDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSyobyoDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRyoyoStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoStartDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRyoyoEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoEndDate;
                            break;
                        case ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfConfirmDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfConfirmDate;
                            break;
                    }
                }
            });


            Result = new Response<ValidHokenInfAllTypeResponse>()
            {
                Data = new ValidHokenInfAllTypeResponse(outputData.Result, outputData.ValidateDetails)
            };
        }
    }
}
