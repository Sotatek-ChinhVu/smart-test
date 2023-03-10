using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.LoadData;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class LoadDataKubunSettingPresenter : ILoadDataKubunSettingOutputPort
    {
        public Response<LoadDataKubunSettingResponse> Result { get; private set; } = default!;

        public void Complete(LoadDataKubunSettingOutputData outputData)
        {
            Result = new Response<LoadDataKubunSettingResponse>()
            {
                Data = new LoadDataKubunSettingResponse(outputData.MaxGrpId, outputData.RaiinKubunList),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case LoadDataKubunSettingStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case LoadDataKubunSettingStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
