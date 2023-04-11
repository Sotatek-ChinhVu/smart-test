using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Diseases;
using UseCase.Diseases.Validation;

namespace EmrCloudApi.Presenters.Diseases
{
    public class ValidationPtDiseaseListPresenter : IValidationPtDiseaseListOutputPort
    {
        public Response<ValidationPtDiseaseListResponse> Result { get; private set; } = new Response<ValidationPtDiseaseListResponse>();

        public void Complete(ValidationPtDiseaseListOutputData output)
        {
            Result.Data = new ValidationPtDiseaseListResponse(output.Status, GetMessage(output.Status));
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(ValidationPtDiseaseListStatus status) => status switch
        {
            ValidationPtDiseaseListStatus.Success => ResponseMessage.PtDiseaseUpsertSuccess,
            ValidationPtDiseaseListStatus.Valid => ResponseMessage.Valid,
            ValidationPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess => ResponseMessage.PtDiseaseUpsertFail,
            ValidationPtDiseaseListStatus.PtDiseaseListInputNoData => ResponseMessage.PtDiseaseUpsertInputNoData,
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn => ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn => ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd => ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtDiseaseListPtIdNoExist => ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist => ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidFreeWord => ResponseMessage.MEnt00040_1,
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue => string.Format(ResponseMessage.MInp00010, ResponseMessage.MTenkiContinue),
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateCommon => string.Format(ResponseMessage.MInp00010, ResponseMessage.MTenkiDate),
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate => string.Format(ResponseMessage.MInp00110, ResponseMessage.MTenkiDate, ResponseMessage.MTenkiStartDate),
            ValidationPtDiseaseListStatus.PtDiseaseListInvalidByomei => string.Format(ResponseMessage.MInp00160_1, ResponseMessage.MDisease),
            ValidationPtDiseaseListStatus.PtInvalidId =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidHpId =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidPtId =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidSortNo =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidByomeiCd =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidStartDate =>
              ResponseMessage.MTenkiStartDate_2,
            ValidationPtDiseaseListStatus.PtInvalidTenkiDate =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidSyubyoKbn =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidHosokuCmt =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidHokenPid =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidIsNodspRece =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidIsNodspKarte =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidSeqNo =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidIsImportant =>
              ResponseMessage.MCommonError,
            ValidationPtDiseaseListStatus.PtInvalidIsDeleted =>
              ResponseMessage.MCommonError,
            _ => string.Empty
        };
    }
}
