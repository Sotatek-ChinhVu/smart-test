using EmrCloudApi.Responses.SpecialNote;
using EmrCloudApi.Responses;
using UseCase.SpecialNote.Save;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using EmrCloudApi.Constants;
using UseCase.SpecialNote.Get;

namespace EmrCloudApi.Presenters.SpecialNote
{
    public class SaveSpecialNotePresenter : ISaveSpecialNoteOutputPort
    {
        public Response<SaveSpecialNoteResponse> Result { get; private set; } = default!;

        public void Complete(SaveSpecialNoteOutputData outputData)
        {
            Result = new Response<SaveSpecialNoteResponse>()
            {
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SaveSpecialNoteStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SaveSpecialNoteStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case SaveSpecialNoteStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case SaveSpecialNoteStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
