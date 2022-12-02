using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.ValidKohi;

namespace EmrCloudApi.Presenters.Insurance
{ 
    public class ValidateKohiPresenter : IValidKohiOutputPort
    {
        public Response<ValidateKohiResponse> Result { get; private set; } = default!;
        public void Complete(ValidKohiOutputData output)
        {
            output.ValidateDetails.ForEach(x =>
            {
                switch (x.Status)
                {
                    case ValidKohiStatus.InvalidFaild:
                        x.Message = ResponseMessage.InvalidException;
                        break;
                    case ValidKohiStatus.InvalidSuccess:
                        x.Message = ResponseMessage.Success;
                        break;
                    case ValidKohiStatus.InvalidSindate:
                        x.Message = ResponseMessage.InvalidSinDate;
                        break;
                    case ValidKohiStatus.InvalidPtBirthday:
                        x.Message = ResponseMessage.InvalidPtBirthday;
                        break;
                    case ValidKohiStatus.InvalidKohiEmptyModel1:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel1;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEmpty1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty1;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNoEmpty1:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty1;
                        break;
                    case ValidKohiStatus.InvalidJyukyusyaNo1:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo1;
                        break;
                    case ValidKohiStatus.InvalidTokusyuNo1:
                        x.Message = ResponseMessage.InvalidTokusyuNo1;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNo01:
                        x.Message = ResponseMessage.InvalidFutansyaNo01;
                        break;
                    case ValidKohiStatus.InvalidKohiYukoDate1:
                        x.Message = ResponseMessage.InvalidKohiYukoDate1;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstStartDate1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate1;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEndDate1:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate1;
                        break;
                    case ValidKohiStatus.InvalidKohiConfirmDate1:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate1;
                        break;
                    case ValidKohiStatus.InvalidMstCheckHBT1:
                        x.Message = ResponseMessage.InvalidMstCheckHBT1;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo1:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo1;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo1:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo1;
                        break;
                    case ValidKohiStatus.InvalidMstCheckAge1:
                        x.Message = ResponseMessage.InvalidMstCheckAge1;
                        break;
                    case ValidKohiStatus.InvalidFutanJyoTokuNull1:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull1;
                        break;
                    case ValidKohiStatus.InvalidKohiEmptyModel2:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel2;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEmpty2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty2;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNoEmpty2:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty2;
                        break;
                    case ValidKohiStatus.InvalidJyukyusyaNo2:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo2;
                        break;
                    case ValidKohiStatus.InvalidTokusyuNo2:
                        x.Message = ResponseMessage.InvalidTokusyuNo2;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNo02:
                        x.Message = ResponseMessage.InvalidFutansyaNo02;
                        break;
                    case ValidKohiStatus.InvalidKohiYukoDate2:
                        x.Message = ResponseMessage.InvalidKohiYukoDate2;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstStartDate2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate2;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEndDate2:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate2;
                        break;
                    case ValidKohiStatus.InvalidKohiConfirmDate2:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate2;
                        break;
                    case ValidKohiStatus.InvalidMstCheckHBT2:
                        x.Message = ResponseMessage.InvalidMstCheckHBT2;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo2:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo2;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo2:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo2;
                        break;
                    case ValidKohiStatus.InvalidMstCheckAge2:
                        Result.Message = ResponseMessage.InvalidMstCheckAge2;
                        break;
                    case ValidKohiStatus.InvalidFutanJyoTokuNull2:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull2;
                        break;
                    case ValidKohiStatus.InvalidKohiEmptyModel3:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel3;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEmpty3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty3;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNoEmpty3:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty3;
                        break;
                    case ValidKohiStatus.InvalidJyukyusyaNo3:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo3;
                        break;
                    case ValidKohiStatus.InvalidTokusyuNo3:
                        x.Message = ResponseMessage.InvalidTokusyuNo3;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNo03:
                        x.Message = ResponseMessage.InvalidFutansyaNo03;
                        break;
                    case ValidKohiStatus.InvalidKohiYukoDate3:
                        x.Message = ResponseMessage.InvalidKohiYukoDate3;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstStartDate3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate3;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEndDate3:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate3;
                        break;
                    case ValidKohiStatus.InvalidKohiConfirmDate3:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate3;
                        break;
                    case ValidKohiStatus.InvalidMstCheckHBT3:
                        x.Message = ResponseMessage.InvalidMstCheckHBT3;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo3:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo3;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo3:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo3;
                        break;
                    case ValidKohiStatus.InvalidMstCheckAge3:
                        x.Message = ResponseMessage.InvalidMstCheckAge3;
                        break;
                    case ValidKohiStatus.InvalidFutanJyoTokuNull3:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull3;
                        break;
                    case ValidKohiStatus.InvalidKohiEmptyModel4:
                        x.Message = ResponseMessage.InvalidKohiEmptyModel4;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEmpty4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEmpty4;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNoEmpty4:
                        x.Message = ResponseMessage.InvalidFutansyaNoEmpty4;
                        break;
                    case ValidKohiStatus.InvalidJyukyusyaNo4:
                        x.Message = ResponseMessage.InvalidJyukyusyaNo4;
                        break;
                    case ValidKohiStatus.InvalidTokusyuNo4:
                        x.Message = ResponseMessage.InvalidTokusyuNo4;
                        break;
                    case ValidKohiStatus.InvalidFutansyaNo04:
                        x.Message = ResponseMessage.InvalidFutansyaNo04;
                        break;
                    case ValidKohiStatus.InvalidKohiYukoDate4:
                        x.Message = ResponseMessage.InvalidKohiYukoDate4;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstStartDate4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstStartDate4;
                        break;
                    case ValidKohiStatus.InvalidKohiHokenMstEndDate4:
                        x.Message = ResponseMessage.InvalidKohiHokenMstEndDate4;
                        break;
                    case ValidKohiStatus.InvalidKohiConfirmDate4:
                        x.Message = ResponseMessage.InvalidKohiConfirmDate4;
                        break;
                    case ValidKohiStatus.InvalidMstCheckHBT4:
                        x.Message = ResponseMessage.InvalidMstCheckHBT4;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo4:
                        x.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo4;
                        break;
                    case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo4:
                        x.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo4;
                        break;
                    case ValidKohiStatus.InvalidMstCheckAge4:
                        x.Message = ResponseMessage.InvalidMstCheckAge4;
                        break;
                    case ValidKohiStatus.InvalidFutanJyoTokuNull4:
                        x.Message = ResponseMessage.InvalidFutanJyoTokuNull4;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty21:
                        x.Message = ResponseMessage.InvalidKohiEmpty21;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty31:
                        x.Message = ResponseMessage.InvalidKohiEmpty31;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty32:
                        x.Message = ResponseMessage.InvalidKohiEmpty32;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty41:
                        x.Message = ResponseMessage.InvalidKohiEmpty41;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty42:
                        x.Message = ResponseMessage.InvalidKohiEmpty42;
                        break;
                    case ValidKohiStatus.InvalidKohiEmpty43:
                        x.Message = ResponseMessage.InvalidKohiEmpty43;
                        break;
                    case ValidKohiStatus.InvalidDuplicateKohi1:
                        x.Message = ResponseMessage.InvalidDuplicateKohi1;
                        break;
                    case ValidKohiStatus.InvalidDuplicateKohi2:
                        x.Message = ResponseMessage.InvalidDuplicateKohi2;
                        break;
                    case ValidKohiStatus.InvalidDuplicateKohi3:
                        x.Message = ResponseMessage.InvalidDuplicateKohi3;
                        break;
                    case ValidKohiStatus.InvalidDuplicateKohi4:
                        x.Message = ResponseMessage.InvalidDuplicateKohi4;
                        break;
                }
            });
            Result = new Response<ValidateKohiResponse>()
            {
                Data = new ValidateKohiResponse(output.Result, output.ValidateDetails)
            };
        }
    }
}
