using Domain.Models.Insurance;
using Domain.Models.RsvInf;
using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.RsvInfToConfirm;

namespace EmrCloudApi.Presenters.MainMenu
{
    public class GetRsvInfToConfirmPresenter : IGetRsvInfToConfirmOutputPort
    {
        public Response<GetRsvInfToConfirmResponse> Result { get; private set; } = new();

        public void Complete(GetRsvInfToConfirmOutputData outputData)
        {
            Result.Data = new GetRsvInfToConfirmResponse(ConvertToRsvInfToConfirmItem(outputData.RsvInfToConfirms));
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetRsvInfToConfirmStatus status) => status switch
        {
            GetRsvInfToConfirmStatus.Successed => ResponseMessage.Success,
            GetRsvInfToConfirmStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };

        private static List<RsvInfToConfirmItem> ConvertToRsvInfToConfirmItem(List<RsvInfToConfirmModel> rsvInfToConfirms)
        {
            var result = new List<RsvInfToConfirmItem>();
            foreach (var item in rsvInfToConfirms)
            {
                result.Add(new RsvInfToConfirmItem(item.PtName, item.HpId, item.SinDate, item.RaiinNo, item.PtId, item.PtNum, item.Birthday, item.TantoId, item.KaId, item.PtNumDisplay, item.Age,
                                                   ConvertToHokenInfItem(item.ListPtHokenInfModel)
                                                    ));
            }

            return result;
        }

        private static List<HokenInfItem> ConvertToHokenInfItem(List<HokenInfModel> hokenInfModels)
        {
            var result = new List<HokenInfItem>();
            foreach (var item in hokenInfModels)
            {
                result.Add(new HokenInfItem(item.PtId, item.HokenId, item.SeqNo, item.HokenNo, item.HokenEdaNo, item.HokenKbn, item.HokensyaNo, item.Kigo, item.Bango, item.EdaNo, item.HonkeKbn, item.KogakuKbn));
            }

            return result;
        }

    }
}
