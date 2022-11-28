using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SpecialNote;
using UseCase.SpecialNote.AddAlrgyDrugList;

namespace EmrCloudApi.Presenters.SpecialNote
{
    public class AddAlrgyDrugListPresenter : IAddAlrgyDrugListOutputPort
    {
        public Response<AddAlrgyDrugListResponse> Result { get; private set; } = default!;

        public void Complete(AddAlrgyDrugListOutputData outputData)
        {
            var reponse = new List<AddAlrgyDrugListItemResponse>();

            Result = new Response<AddAlrgyDrugListResponse>()
            {
                Message = !outputData.KeyPairValues.Any() ? ResponseMessage.Success : ResponseMessage.Failed,
                Status = !outputData.KeyPairValues.Any() ? 1 : 2
            };
            foreach (var validation in outputData.KeyPairValues)
            {
                var value = validation.Value;
                switch (value)
                {
                    case AddAlrgyDrugListStatus.InvalidCmt:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidCmt));
                        break;
                    case AddAlrgyDrugListStatus.InvalidPtId:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidPtId));
                        break;
                    case AddAlrgyDrugListStatus.HpIdNoExist:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugHpIdNoExist));
                        break;
                    case AddAlrgyDrugListStatus.PtIdNoExist:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugPtIdNoExist));
                        break;
                    case AddAlrgyDrugListStatus.InvalidItemCd:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidItemCd));
                        break;
                    case AddAlrgyDrugListStatus.InvalidSortNo:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidSortNo));
                        break;
                    case AddAlrgyDrugListStatus.InvalidStartDate:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidStartDate));
                        break;
                    case AddAlrgyDrugListStatus.InvalidEndDate:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidEndDate));
                        break;
                    case AddAlrgyDrugListStatus.InvalidDrugName:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInvalidDrugName));
                        break;
                    case AddAlrgyDrugListStatus.InvalidDuplicate:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugDuplicate));
                        break;
                    case AddAlrgyDrugListStatus.InputNoData:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugInputNoData));
                        break;
                    case AddAlrgyDrugListStatus.ItemCdNoExist:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.AddAlrgyDrugItemCd));
                        break;
                    case AddAlrgyDrugListStatus.Failed:
                        reponse.Add(new AddAlrgyDrugListItemResponse(value, validation.Key, ResponseMessage.Failed));
                        break;
                    default:
                        reponse.Add(new AddAlrgyDrugListItemResponse(AddAlrgyDrugListStatus.Successed, -1, ResponseMessage.Success));
                        break;
                }
            }

            Result.Data = new AddAlrgyDrugListResponse(reponse);
        }
    }
}
