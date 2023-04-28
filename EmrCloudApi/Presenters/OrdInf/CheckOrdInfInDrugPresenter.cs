using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.OrdInfs;
using UseCase.OrdInfs.CheckOrdInfInDrug;

namespace EmrCloudApi.Presenters.OrdInfs
{
    public class CheckOrdInfInDrugPresenter : ICheckOrdInfInDrugOutputPort
    {
        public Response<CheckOrdInfInDrugResponse> Result { get; private set; } = default!;

        public void Complete(CheckOrdInfInDrugOutputData outputData)
        {
            Result = new Response<CheckOrdInfInDrugResponse>()
            {
                Data = new CheckOrdInfInDrugResponse(outputData.Result),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CheckOrdInfInDrugStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case CheckOrdInfInDrugStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CheckOrdInfInDrugStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CheckOrdInfInDrugStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
