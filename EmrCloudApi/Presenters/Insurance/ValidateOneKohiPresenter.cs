using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidateOneKohi;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateOneKohiPresenter : IValidOneKohiOutputPort
    {
        public Response<ValidateOneKohiResponse> Result { get; private set; } = default!;
        public void Complete(ValidOneKohiOutputData output)
        {
            output.ValidateDetails.ForEach(x =>
            {
                switch (x.Status)
                {
                    case ValidOneKohiStatus.InvalidFaild:
                        x.Message = ResponseMessage.InvalidException;
                        break;
                    case ValidOneKohiStatus.InvalidSuccess:
                        x.Message = ResponseMessage.Success;
                        break;
                    case ValidOneKohiStatus.InvalidSindate:
                        x.Message = ResponseMessage.InvalidSinDate;
                        break;
                    case ValidOneKohiStatus.InvalidPtBirthday:
                        x.Message = ResponseMessage.InvalidPtBirthday;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmptyModel1:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel1;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEmpty1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty1;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNoEmpty1:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty1;
                        break;
                    case ValidOneKohiStatus.InvalidJyukyusyaNo1:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo1;
                        break;
                    case ValidOneKohiStatus.InvalidTokusyuNo1:
                        x.Message = ResponseMessage.InvalidTokusyuNo1;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNo01:
                        x.Message = ResponseMessage.InvalidFutansyaNo01;
                        break;
                    case ValidOneKohiStatus.InvalidKohiYukoDate1:
                        x.Message = ResponseMessage.InvalidKohiYukoDate1;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstStartDate1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate1;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEndDate1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate1;
                        break;
                    case ValidOneKohiStatus.InvalidKohiConfirmDate1:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate1;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckHBT1:
                        x.Message = ResponseMessage.InvalidMstCheckHBT1;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo1:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo1;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo1:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo1;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckAge1:
                        x.Message = ResponseMessage.InvalidMstCheckAge1;
                        break;
                    case ValidOneKohiStatus.InvalidFutanJyoTokuNull1:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull1;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmptyModel2:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel2;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEmpty2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty2;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNoEmpty2:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty2;
                        break;
                    case ValidOneKohiStatus.InvalidJyukyusyaNo2:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo2;
                        break;
                    case ValidOneKohiStatus.InvalidTokusyuNo2:
                        x.Message = ResponseMessage.InvalidTokusyuNo2;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNo02:
                        x.Message = ResponseMessage.InvalidFutansyaNo02;
                        break;
                    case ValidOneKohiStatus.InvalidKohiYukoDate2:
                        x.Message = ResponseMessage.InvalidKohiYukoDate2;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstStartDate2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate2;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEndDate2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate2;
                        break;
                    case ValidOneKohiStatus.InvalidKohiConfirmDate2:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate2;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckHBT2:
                        x.Message = ResponseMessage.InvalidMstCheckHBT2;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo2:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo2;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo2:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo2;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckAge2:
                        Result.Message = ResponseMessage.InvalidMstCheckAge2;
                        break;
                    case ValidOneKohiStatus.InvalidFutanJyoTokuNull2:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull2;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmptyModel3:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel3;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEmpty3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty3;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNoEmpty3:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty3;
                        break;
                    case ValidOneKohiStatus.InvalidJyukyusyaNo3:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo3;
                        break;
                    case ValidOneKohiStatus.InvalidTokusyuNo3:
                        x.Message = ResponseMessage.InvalidTokusyuNo3;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNo03:
                        x.Message = ResponseMessage.InvalidFutansyaNo03;
                        break;
                    case ValidOneKohiStatus.InvalidKohiYukoDate3:
                        x.Message = ResponseMessage.InvalidKohiYukoDate3;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstStartDate3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate3;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEndDate3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate3;
                        break;
                    case ValidOneKohiStatus.InvalidKohiConfirmDate3:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate3;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckHBT3:
                        x.Message = ResponseMessage.InvalidMstCheckHBT3;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo3:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo3;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo3:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo3;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckAge3:
                        x.Message = ResponseMessage.InvalidMstCheckAge3;
                        break;
                    case ValidOneKohiStatus.InvalidFutanJyoTokuNull3:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull3;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmptyModel4:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel4;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEmpty4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty4;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNoEmpty4:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty4;
                        break;
                    case ValidOneKohiStatus.InvalidJyukyusyaNo4:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo4;
                        break;
                    case ValidOneKohiStatus.InvalidTokusyuNo4:
                        x.Message = ResponseMessage.InvalidTokusyuNo4;
                        break;
                    case ValidOneKohiStatus.InvalidFutansyaNo04:
                        x.Message = ResponseMessage.InvalidFutansyaNo04;
                        break;
                    case ValidOneKohiStatus.InvalidKohiYukoDate4:
                        x.Message = ResponseMessage.InvalidKohiYukoDate4;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstStartDate4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate4;
                        break;
                    case ValidOneKohiStatus.InvalidKohiHokenMstEndDate4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate4;
                        break;
                    case ValidOneKohiStatus.InvalidKohiConfirmDate4:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate4;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckHBT4:
                        x.Message = ResponseMessage.InvalidMstCheckHBT4;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo4:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo4;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo4:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo4;
                        break;
                    case ValidOneKohiStatus.InvalidMstCheckAge4:
                        x.Message = ResponseMessage.InvalidMstCheckAge4;
                        break;
                    case ValidOneKohiStatus.InvalidFutanJyoTokuNull4:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull4;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty21:
                        x.Message = ResponseMessage.InvalidKohiEmpty21;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty31:
                        x.Message = ResponseMessage.InvalidKohiEmpty31;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty32:
                        x.Message = ResponseMessage.InvalidKohiEmpty32;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty41:
                        x.Message = ResponseMessage.InvalidKohiEmpty41;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty42:
                        x.Message = ResponseMessage.InvalidKohiEmpty42;
                        break;
                    case ValidOneKohiStatus.InvalidKohiEmpty43:
                        x.Message = ResponseMessage.InvalidKohiEmpty43;
                        break;
                    case ValidOneKohiStatus.InvalidDuplicateKohi1:
                        x.Message = ResponseMessage.InvalidDuplicateKohi1;
                        break;
                    case ValidOneKohiStatus.InvalidDuplicateKohi2:
                        x.Message = ResponseMessage.InvalidDuplicateKohi2;
                        break;
                    case ValidOneKohiStatus.InvalidDuplicateKohi3:
                        x.Message = ResponseMessage.InvalidDuplicateKohi3;
                        break;
                    case ValidOneKohiStatus.InvalidDuplicateKohi4:
                        x.Message = ResponseMessage.InvalidDuplicateKohi4;
                        break;
                }
            });
            Result = new Response<ValidateOneKohiResponse>()
            {
                Data = new ValidateOneKohiResponse(output.Result, output.ValidateDetails)
            };
        }
    }
}
