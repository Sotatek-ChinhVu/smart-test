using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetJihiSbtMstList;
using UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetSingleDoseMstAndMedicineUnitListPresenter : IGetSingleDoseMstAndMedicineUnitListOutputPort
    {
        public Response<GetSingleDoseMstAndMedicineUnitResponse> Result { get; private set; } = default!;

        public void Complete(GetSingleDoseMstAndMedicineUnitListOutputData outputData)
        {
            Result = new Response<GetSingleDoseMstAndMedicineUnitResponse>
            {
                Data = new GetSingleDoseMstAndMedicineUnitResponse(outputData.SingleDoseMsts, outputData.MedicineUnits),
                Status = outputData.Status == true ? 1 : 0,
            };
            switch (outputData.Status)
            {
                case true:
                    Result.Message = ResponseMessage.Success;
                    break;
                case false:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
