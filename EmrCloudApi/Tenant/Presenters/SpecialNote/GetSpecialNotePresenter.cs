using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using UseCase.SpecialNote.Read;

namespace EmrCloudApi.Tenant.Presenters.SpecialNote
{
    public class GetSpecialNotePresenter
    {
        public Response<GetSpecialNoteResponse> Result { get; private set; } = default!;

        public void Complete(GetSpecialNoteOutputData outputData)
        {
            Result = new Response<GetSpecialNoteResponse>()
            {
                Data = new GetSpecialNoteResponse()
                {
                    SpecialNote = outputData.SpecialNoteDTO
                },
                Status = 1,
                Message = ResponseMessage.GetSpecialNoteSuccessed
            };
        }
    }
}
