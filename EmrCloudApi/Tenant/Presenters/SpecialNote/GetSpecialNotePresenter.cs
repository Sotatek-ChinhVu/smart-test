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
                Data = new GetSpecialNoteResponse(null, null, null),
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
