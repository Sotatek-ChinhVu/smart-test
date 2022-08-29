using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using UseCase.SpecialNote.Get;

namespace EmrCloudApi.Tenant.Presenters.SpecialNote
{
    public class GetSpecialNotePresenter : IGetSpecialNoteOutputPort
    {
        public Response<GetSpecialNoteResponse> Result { get; private set; } = default!;

        public void Complete(GetSpecialNoteOutputData outputData)
        {
            Result = new Response<GetSpecialNoteResponse>()
            {
                Data = new GetSpecialNoteResponse(new SummaryInfModel(), new ImportantNoteModel(), new PatientInfoModel()),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSpecialNoteStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSpecialNoteStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetSpecialNoteStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetSpecialNoteStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    Result.Data = new GetSpecialNoteResponse(outputData.SummaryTab, outputData.ImportantNoteTab, outputData.PatientInfoTab);
                    break;
            }
        }
    }
}
