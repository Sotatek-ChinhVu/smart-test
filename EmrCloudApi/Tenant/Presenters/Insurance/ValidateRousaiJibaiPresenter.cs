using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidateRousaiJibai;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateRousaiJibaiPresenter : IValidateRousaiJibaiOutputPort
    {
        public Response<ValidateRousaiJibaiResponse> Result { get; private set; } = default!;
        public void Complete(ValidateRousaiJibaiOutputData output)
        {
            output.ValidateDetails.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Message))
                {
                    switch (x.Status)
                    {
                        case ValidateRousaiJibaiStatus.InvalidSuccess:
                            x.Message = ResponseMessage.Success;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidHokenKbn:
                            x.Message = ResponseMessage.InvalidHokenKbn;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSinDate:
                            x.Message = ResponseMessage.InvalidSinDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSaigaiKbn:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSaigaiKbn;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSyobyoDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSyobyoDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoStartDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoEndDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfStartDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfEndDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfConfirmDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidHpId:
                            x.Message = ResponseMessage.InvalidHpId;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidFaild:
                            x.Message = ResponseMessage.Failed;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidRodoBangoNull:
                            x.Message = ResponseMessage.InvalidRodoBangoNull;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidRodoBangoLengthNotEquals14:
                            x.Message = ResponseMessage.InvalidRodoBangoLengthNotEquals14;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckItemFirstListRousaiTenki:
                            x.Message = ResponseMessage.InvalidCheckItemFirstListRousaiTenki;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiSinkei:
                            x.Message = ResponseMessage.InvalidCheckRousaiTenkiSinkei;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiTenki:
                            x.Message = ResponseMessage.InvalidCheckRousaiTenkiTenki;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiEndDate:
                            x.Message = ResponseMessage.InvalidCheckRousaiTenkiEndDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiSaigaiKbnNotEquals1And2:
                            x.Message = ResponseMessage.InvalidCheckRousaiSaigaiKbnNotEquals1And2;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiSyobyoDateEquals0:
                            x.Message = ResponseMessage.InvalidCheckRousaiSyobyoDateEquals0;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull:
                            x.Message = ResponseMessage.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckRousaiRyoyoDate:
                            x.Message = ResponseMessage.InvalidCheckRousaiRyoyoDate;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidCheckDateExpirated:
                            x.Message = ResponseMessage.InvalidCheckDateExpirated;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidNenkinBangoIsNull:
                            x.Message = ResponseMessage.InvalidNenkinBangoIsNull;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidNenkinBangoLengthNotEquals9:
                            x.Message = ResponseMessage.InvalidNenkinBangoLengthNotEquals9;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoIsNull:
                            x.Message = ResponseMessage.InvalidKenkoKanriBangoIsNull;
                            break;
                        case ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoLengthNotEquals13:
                            x.Message = ResponseMessage.InvalidKenkoKanriBangoLengthNotEquals13;
                            break;
                    }
                }
            });

            Result = new Response<ValidateRousaiJibaiResponse>()
            {
                Data = new ValidateRousaiJibaiResponse(output.Result, output.ValidateDetails)
            };

        }
    }
}
