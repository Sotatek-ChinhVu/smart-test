using Domain.Constant;
using Domain.Models.Yousiki;
using Entity.Tenant;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;
using UseCase.Yousiki.UpdateYosiki;

namespace Interactor.Yousiki.UpdateYosiki
{
    public class UpdateYosikiInteractor : RepositoryBase, IUpdateYosikiInputPort
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IYousikiRepository _yousikiRepository;

        public UpdateYosikiInteractor(IYousikiRepository yousikiRepository, ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _yousikiRepository = yousikiRepository;
            _tenantProvider = tenantProvider;
        }
        public UpdateYosikiOutputData Handle(UpdateYosikiInputData inputData)
        {
            try
            {

            }
            finally
            {

            }
        }

        public Dictionary<bool, string> ValidationData(int hpId, string codeNo, int rowNo, int payLoad, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfDetailModel yousiki1InfDetailModel, Yousiki1InfModel yousiki1InfModel, string valueDefault = "")
        {
            Dictionary<bool, string> result = new();

            if (GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 1, yousiki1InfDetailModels, yousiki1InfDetailModel).Value.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 生年月日");
                result.Add(false, message);

                return result;
            }

            if (GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 2, yousiki1InfDetailModels, yousiki1InfDetailModel).Value.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 性別");
                result.Add(false, message);

                return result;
            }

            var homePostModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 2, yousiki1InfDetailModels, yousiki1InfDetailModel).Value;

            if (string.IsNullOrEmpty(homePostModel))
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 郵便番号");
                result.Add(false, message);

                return result;
            }

            var bodyHeight = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_HeightAndWeight, 0, 1, yousiki1InfDetailModels, yousiki1InfDetailModel).Value ?? string.Empty;
            var bodyWeight = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_HeightAndWeight, 0, 2, yousiki1InfDetailModels, yousiki1InfDetailModel).Value ?? string.Empty;

            if (!bodyHeight.Equals("000") && bodyHeight.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 身長・体重 - 身長");
                result.Add(false, message);

                return result;
            }

            if (!bodyWeight.Equals("000") && bodyWeight.AsDouble() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 身長・体重 - 体重");
                result.Add(false, message);

                return result;
            }

            var smokingTypeModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 1, yousiki1InfDetailModels, yousiki1InfDetailModel, "0");
            var smokingNumberOfDayModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 2, yousiki1InfDetailModels, yousiki1InfDetailModel); 
            var smokingYearModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 3, yousiki1InfDetailModels, yousiki1InfDetailModel);

            if (smokingTypeModel.Value.AsInteger() > 0)
            {
                if (string.IsNullOrEmpty(smokingNumberOfDayModel.Value) || (!smokingNumberOfDayModel.Value.Equals("000") && smokingNumberOfDayModel.Value.AsInteger() <= 0))
                {
                    string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 喫煙歴 - 1日の喫煙本数");
                    IsErrorSmokingNumberOfDay = true;
                    return false;
                }
            }
            return result;
        }

        private Yousiki1InfDetailModel GetYousiki1InfDetailModel(int hpId, string codeNo, int rowNo, int payLoad, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfDetailModel yousiki1InfDetailModel, string valueDefault = "")
        {
            Yousiki1InfDetailModel detail;
            if (yousiki1InfDetailModels == null || yousiki1InfDetailModels.Count == 0)
            {
                detail = CreateYousiki1InfDetailModel(hpId, codeNo, rowNo, payLoad, yousiki1InfDetailModel, valueDefault);
            }
            else
            {
                detail = yousiki1InfDetailModels.FirstOrDefault(x => x.CodeNo == Yousiki1InfDetailConstant.CodeNo_Attributes && x.RowNo == yousiki1InfDetailModel.RowNo && x.Payload == yousiki1InfDetailModel.Payload) ?? new();
                if (detail == null || detail == new Yousiki1InfDetailModel())
                {
                    detail = CreateYousiki1InfDetailModel(hpId, codeNo, rowNo, payLoad, yousiki1InfDetailModel, valueDefault);
                }
            }
            yousiki1InfDetailModels.Add(detail);
            return detail;
        }

        private Yousiki1InfDetailModel CreateYousiki1InfDetailModel(int hpId, string codeNo, int rowNo, int payLoad, Yousiki1InfDetailModel yousiki1InfDetailModel, string valueDefault = "")
        {
            var yousiki1Inf = NoTrackingDataContext.Yousiki1InfDetails.Where(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == codeNo && x.RowNo == rowNo && x.Payload == payLoad).First();
            if (yousiki1Inf == null)
            {
                return new();
            }

            var detail = new Yousiki1InfDetailModel(
                        yousiki1Inf.PtId,
                        yousiki1Inf.SinYm,
                        yousiki1Inf.DataType,
                        yousiki1Inf.SeqNo,
                        yousiki1Inf.CodeNo ?? string.Empty,
                        yousiki1Inf.RowNo,
                        yousiki1Inf.Payload,
                        yousiki1Inf.Value ?? string.Empty);

            return detail;
        }
    }
}
