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
            Result = new Response<ValidateKohiResponse>()
            {
                Data = new ValidateKohiResponse(output.Result, output.Message , output.TypeMessage),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case ValidKohiStatus.InvalidFaild:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ValidKohiStatus.InvalidSuccess:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidKohiStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidKohiStatus.InvalidPtBirthday:
                    Result.Message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidKohiStatus.InvalidKohiEmptyModel1:
                    Result.Message = ResponseMessage.InvalidKohiEmptyModel1;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEmpty1:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEmpty1;
                    break;
                case ValidKohiStatus.InvalidFutansyaNoEmpty1:
                    Result.Message = ResponseMessage.InvalidFutansyaNoEmpty1;
                    break;
                case ValidKohiStatus.InvalidJyukyusyaNo1:
                    Result.Message = ResponseMessage.InvalidJyukyusyaNo1;
                    break;
                case ValidKohiStatus.InvalidTokusyuNo1:
                    Result.Message = ResponseMessage.InvalidTokusyuNo1;
                    break;
                case ValidKohiStatus.InvalidFutansyaNo01:
                    Result.Message = ResponseMessage.InvalidFutansyaNo01;
                    break;
                case ValidKohiStatus.InvalidKohiYukoDate1:
                    Result.Message = ResponseMessage.InvalidKohiYukoDate1;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstStartDate1:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstStartDate1;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEndDate1:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEndDate1;
                    break;
                case ValidKohiStatus.InvalidKohiConfirmDate1:
                    Result.Message = ResponseMessage.InvalidKohiConfirmDate1;
                    break;
                case ValidKohiStatus.InvalidMstCheckHBT1:
                    Result.Message = ResponseMessage.InvalidMstCheckHBT1;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo1:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo1;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo1:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo1;
                    break;
                case ValidKohiStatus.InvalidMstCheckAge1:
                    Result.Message = ResponseMessage.InvalidMstCheckAge1;
                    break;
                case ValidKohiStatus.InvalidFutanJyoTokuNull1:
                    Result.Message = ResponseMessage.InvalidFutanJyoTokuNull1;
                    break;
                case ValidKohiStatus.InvalidKohiEmptyModel2:
                    Result.Message = ResponseMessage.InvalidKohiEmptyModel2;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEmpty2:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEmpty2;
                    break;
                case ValidKohiStatus.InvalidFutansyaNoEmpty2:
                    Result.Message = ResponseMessage.InvalidFutansyaNoEmpty2;
                    break;
                case ValidKohiStatus.InvalidJyukyusyaNo2:
                    Result.Message = ResponseMessage.InvalidJyukyusyaNo2;
                    break;
                case ValidKohiStatus.InvalidTokusyuNo2:
                    Result.Message = ResponseMessage.InvalidTokusyuNo2;
                    break;
                case ValidKohiStatus.InvalidFutansyaNo02:
                    Result.Message = ResponseMessage.InvalidFutansyaNo02;
                    break;
                case ValidKohiStatus.InvalidKohiYukoDate2:
                    Result.Message = ResponseMessage.InvalidKohiYukoDate2;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstStartDate2:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstStartDate2;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEndDate2:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEndDate2;
                    break;
                case ValidKohiStatus.InvalidKohiConfirmDate2:
                    Result.Message = ResponseMessage.InvalidKohiConfirmDate2;
                    break;
                case ValidKohiStatus.InvalidMstCheckHBT2:
                    Result.Message = ResponseMessage.InvalidMstCheckHBT2;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo2:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo2;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo2:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo2;
                    break;
                case ValidKohiStatus.InvalidMstCheckAge2:
                    Result.Message = ResponseMessage.InvalidMstCheckAge2;
                    break;
                case ValidKohiStatus.InvalidFutanJyoTokuNull2:
                    Result.Message = ResponseMessage.InvalidFutanJyoTokuNull2;
                    break;
                case ValidKohiStatus.InvalidKohiEmptyModel3:
                    Result.Message = ResponseMessage.InvalidKohiEmptyModel3;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEmpty3:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEmpty3;
                    break;
                case ValidKohiStatus.InvalidFutansyaNoEmpty3:
                    Result.Message = ResponseMessage.InvalidFutansyaNoEmpty3;
                    break;
                case ValidKohiStatus.InvalidJyukyusyaNo3:
                    Result.Message = ResponseMessage.InvalidJyukyusyaNo3;
                    break;
                case ValidKohiStatus.InvalidTokusyuNo3:
                    Result.Message = ResponseMessage.InvalidTokusyuNo3;
                    break;
                case ValidKohiStatus.InvalidFutansyaNo03:
                    Result.Message = ResponseMessage.InvalidFutansyaNo03;
                    break;
                case ValidKohiStatus.InvalidKohiYukoDate3:
                    Result.Message = ResponseMessage.InvalidKohiYukoDate3;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstStartDate3:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstStartDate3;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEndDate3:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEndDate3;
                    break;
                case ValidKohiStatus.InvalidKohiConfirmDate3:
                    Result.Message = ResponseMessage.InvalidKohiConfirmDate3;
                    break;
                case ValidKohiStatus.InvalidMstCheckHBT3:
                    Result.Message = ResponseMessage.InvalidMstCheckHBT3;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo3:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo3;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo3:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo3;
                    break;
                case ValidKohiStatus.InvalidMstCheckAge3:
                    Result.Message = ResponseMessage.InvalidMstCheckAge3;
                    break;
                case ValidKohiStatus.InvalidFutanJyoTokuNull3:
                    Result.Message = ResponseMessage.InvalidFutanJyoTokuNull3;
                    break;
                case ValidKohiStatus.InvalidKohiEmptyModel4:
                    Result.Message = ResponseMessage.InvalidKohiEmptyModel4;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEmpty4:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEmpty4;
                    break;
                case ValidKohiStatus.InvalidFutansyaNoEmpty4:
                    Result.Message = ResponseMessage.InvalidFutansyaNoEmpty4;
                    break;
                case ValidKohiStatus.InvalidJyukyusyaNo4:
                    Result.Message = ResponseMessage.InvalidJyukyusyaNo4;
                    break;
                case ValidKohiStatus.InvalidTokusyuNo4:
                    Result.Message = ResponseMessage.InvalidTokusyuNo4;
                    break;
                case ValidKohiStatus.InvalidFutansyaNo04:
                    Result.Message = ResponseMessage.InvalidFutansyaNo04;
                    break;
                case ValidKohiStatus.InvalidKohiYukoDate4:
                    Result.Message = ResponseMessage.InvalidKohiYukoDate4;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstStartDate4:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstStartDate4;
                    break;
                case ValidKohiStatus.InvalidKohiHokenMstEndDate4:
                    Result.Message = ResponseMessage.InvalidKohiHokenMstEndDate4;
                    break;
                case ValidKohiStatus.InvalidKohiConfirmDate4:
                    Result.Message = ResponseMessage.InvalidKohiConfirmDate4;
                    break;
                case ValidKohiStatus.InvalidMstCheckHBT4:
                    Result.Message = ResponseMessage.InvalidMstCheckHBT4;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitFutansyaNo4:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitFutansyaNo4;
                    break;
                case ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo4:
                    Result.Message = ResponseMessage.InvalidMstCheckDigitJyukyusyaNo4;
                    break;
                case ValidKohiStatus.InvalidMstCheckAge4:
                    Result.Message = ResponseMessage.InvalidMstCheckAge4;
                    break;
                case ValidKohiStatus.InvalidFutanJyoTokuNull4:
                    Result.Message = ResponseMessage.InvalidFutanJyoTokuNull4;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty21:
                    Result.Message = ResponseMessage.InvalidKohiEmpty21;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty31:
                    Result.Message = ResponseMessage.InvalidKohiEmpty31;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty32:
                    Result.Message = ResponseMessage.InvalidKohiEmpty32;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty41:
                    Result.Message = ResponseMessage.InvalidKohiEmpty41;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty42:
                    Result.Message = ResponseMessage.InvalidKohiEmpty42;
                    break;
                case ValidKohiStatus.InvalidKohiEmpty43:
                    Result.Message = ResponseMessage.InvalidKohiEmpty43;
                    break;
                case ValidKohiStatus.InvalidDuplicateKohi1:
                    Result.Message = ResponseMessage.InvalidDuplicateKohi1;
                    break;
                case ValidKohiStatus.InvalidDuplicateKohi2:
                    Result.Message = ResponseMessage.InvalidDuplicateKohi2;
                    break;
                case ValidKohiStatus.InvalidDuplicateKohi3:
                    Result.Message = ResponseMessage.InvalidDuplicateKohi3;
                    break;
                case ValidKohiStatus.InvalidDuplicateKohi4:
                    Result.Message = ResponseMessage.InvalidDuplicateKohi4;
                    break;
            }
        }
    }
}
